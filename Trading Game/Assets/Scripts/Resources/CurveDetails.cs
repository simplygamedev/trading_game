using UnityEngine;

[CreateAssetMenu(fileName = "CurveDetail", menuName = "ScriptableObjects/CurveDetails", order = 0)]
public class CurveDetails : ScriptableObject
{
	public float gradientConstant;
	public float x_displacement;

	public int equilibrium_x;
	public int equilibrium_y;
	public int y_intercept;
}