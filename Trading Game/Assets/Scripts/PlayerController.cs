using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Player player = new Player();

    private string town = "Luebeck";

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnGUI()
    {
        GUILayout.Box(string.Format("Player Gold:{0}", player.gold));
        GUILayout.Box(string.Format("Selected Town:{0}", town));
        foreach (var resource in Market.resourceNames)
        {
            showResource(town, resource);
        }

        if (GUILayout.Button("Change to Gdansk"))
        {
            town = "Gdansk";
        }
        else if (GUILayout.Button("Change to Luebeck"))
        {
            town = "Luebeck";
        }
    }

    public void showResource(string town, string resource)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Box(string.Format("{2} Quantity:{0} {2} Price:{1}", Market.markets[town].getQuantity(resource), Market.markets[town].getPrice(resource, 1), resource));
        GUILayout.Space(1f);
        if (GUILayout.Button("Buy"))
        {
            if (player.BuyResource(resource, Mathf.CeilToInt(Market.markets[town].getPrice(resource, 1)), 1))
            {
                Market.markets[town].DecreaseQuantity(resource, 1);
            }
        }
        if (GUILayout.Button("Sell"))
        {
            if (player.SellResources(resource, Mathf.FloorToInt(Market.markets[town].getPrice(resource, 1)), 1))
            {
                Market.markets[town].IncreaseQuantity(resource, 1);
            }
        }
        GUILayout.EndHorizontal();
    }
}
