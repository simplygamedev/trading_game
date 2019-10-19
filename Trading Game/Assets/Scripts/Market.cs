using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Market : MonoBehaviour {

	public Dictionary<string, Resources> resourceList = new Dictionary<string, Resources>();

	private static readonly string[,] resources = {{"grain", "StraightCurve"}, {"beer", "StraightCurve"}, {"iron", "XSquareCurve"}};

	public CurveDetails grainDetails;
	public CurveDetails beerDetails;
	public CurveDetails ironDetails;

	public int grainInitialAmount = 30;
	public int beerInitialAmount = 30;
	public int ironInitialAmount = 30;

	public int grainConsumption = 3;
	public int beerConsumption = 5;
	public int ironConsumption = 3;

	public int grainProduction = 1;
	public int beerProduction = 1;
	public int ironProduction = 1;

	private System.Collections.IEnumerator resourceUpdater;

	public static string[] stateAvailableResources(){
		string[] resourceList = new string[resources.GetLength(0)];
		for (var i = 0; i < resources.GetLength (0); i++) {
			resourceList[i] = resources [i, 0];
		}
		return resourceList;
	}

	void Awake () 
	{
		//resourceList.Add ("Grain", createNewResourceByName("Grain", grainInitialAmount, grainDetails.equilibrium_x, new StraightCurve(grainDetails)));
		//resourceList.Add ("Beer", createNewResourceByName("Beer", beerInitialAmount, BeerDetails.equilibrium_x, new StraightCurve(BeerDetails)));
		//resourceList.Add ("Iron", createNewResourceByName("Iron", ironInitialAmount, IronDetails.equilibrium_x, new XSquareCurve(IronDetails)));

		for(var i=0; i<resources.GetLength(0); i++){
			
			string resourceName = resources [i, 0];
			int initAmt = GetAttribute<int> (String.Format ("{0}InitialAmount", resourceName));

			CurveDetails curveDetails = GetAttribute<CurveDetails> (String.Format ("{0}Details", resourceName));
			Curve curve = createCurveByName (resources [i, 1], curveDetails);

			Resources resourceToAdd = createNewResourceByName (resourceName.Substring(0,1).ToUpper() + resourceName.Substring(1), initAmt, curveDetails.equilibrium_x, curve);

			resourceList.Add (resourceName, resourceToAdd);
		}
	}

	public void startResourceUpdate(float updateEverySecond){
		if (resourceUpdater == null) {
			resourceUpdater = MarketResourceUpdate (updateEverySecond);
			StartCoroutine (resourceUpdater);
		}  
	}

	public void stopResourceUpdate(){
		if(resourceUpdater!=null){
			StopCoroutine (resourceUpdater);
			resourceUpdater = null;
		}
	}

	public bool isResourceUpdating(){
		return resourceUpdater != null;
	}
		

	public T GetAttribute<T> ( string _name ) {
		return (T)typeof(Market).GetField( _name ).GetValue (this);
	} 

	private Resources createNewResourceByName(string resourceName, int initialAmount, int equilibrium_quantity, Curve curve)
	{
		return (Resources) Activator.CreateInstance (Type.GetType (resourceName), initialAmount, equilibrium_quantity, curve); 
	}

	private Curve createCurveByName(string curveName, CurveDetails curveDetails)
	{
		return (Curve) Activator.CreateInstance (Type.GetType (curveName), new curvedetail(curveDetails)); 
	}

	public bool DecreaseQuantity(string resource, int quantity)
	{
		int currentQuantity = resourceList[resource].getState();

		if (resourceList.ContainsKey (resource) && currentQuantity > 0 && currentQuantity - quantity >= 0) {
			resourceList [resource].minusResources (quantity);	
			return true;
		}
		return false;
	}

	public bool IncreaseQuantity(string resource, int quantity)
	{
		if (resourceList.ContainsKey (resource)) {
			resourceList [resource].increaseResources (quantity);
			return true;
		}
		return false;
	}

	private System.Collections.IEnumerator MarketResourceUpdate(float waitTime)
	{
		while (true) {

			yield return new WaitForSeconds(waitTime);

			foreach (var keypair in resourceList) {

				if (rand (0, 10) > 4)
					ResourceInflow (keypair.Key);
				else
					ResourceOutflow (keypair.Key);

				yield return new WaitForSeconds(waitTime);
			}
		}
	}

	private void ResourceInflow(string resourceName)
	{
		Type type = typeof(Market);
		FieldInfo fieldInfo = type.GetField (String.Format("{0}Production", resourceName.ToLower ()));
		resourceList[resourceName].increaseResources (rand(1, (int)fieldInfo.GetValue (this)));
	}

	private void ResourceOutflow(string resourceName)
	{
		Type type = typeof(Market);
		FieldInfo fieldInfo = type.GetField (String.Format("{0}Consumption", resourceName.ToLower ()));
		resourceList[resourceName].minusResources (rand(1, (int)fieldInfo.GetValue (this)));
	}

	private int rand(int min, int max){
		return UnityEngine.Random.Range (min, max);
	}
}

public struct curvedetail{
	public float gradientConstant;
	public float x_displacement;

	public int equilibrium_x;
	public int equilibrium_y;
	public int y_intercept;

	public curvedetail(CurveDetails curveDetail){
		this.gradientConstant = curveDetail.gradientConstant;
		this.x_displacement = curveDetail.x_displacement;
		this.equilibrium_x = curveDetail.equilibrium_x;
		this.equilibrium_y = curveDetail.equilibrium_y;
		this.y_intercept = curveDetail.y_intercept;
	}
}