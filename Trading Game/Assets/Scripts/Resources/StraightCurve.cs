using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Gradient Constant should always be negative
/// </summary>

public class StraightCurve : Curve 
{
	public StraightCurve (curvedetail curveDetails) : base (curveDetails){}
		
	public override int calculateY(int x){
		return (int)(curveDetails.gradientConstant * x + y_displacement);
	}

	protected override int calculateYDisplacement(){
		return (int)(curveDetails.equilibrium_y - curveDetails.gradientConstant * curveDetails.equilibrium_x);
	}

	public override float GetGradient(){
		return curveDetails.gradientConstant;
	}

	public override float calculateAreaUnderCurve(int initialX, int finalX){	
		return (float)((curveDetails.gradientConstant * Math.Pow (initialX, 2) / 2 + y_displacement * initialX) - (curveDetails.gradientConstant * Math.Pow (finalX, 2) / 2 + y_displacement * finalX));
	}

}
