using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

/// <summary>
/// https://www.raywenderlich.com/9454-introduction-to-unity-unit-testing
/// </summary>
public class TestSuite {

    Market marketscript;

    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject = Object.Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Market"));
        marketscript = gameGameObject.GetComponent<Market>();
    }

    [Test]
    public void CurveTest() {

        float x_displacement = 0;
        float equilibrium_price = 200;
        float equilibrium_qty = 100;
        float price_elasticity = -0.01f;

        Curve xSquaredCurve = Curve.createCurveByName("XSquareCurve", price_elasticity, x_displacement, equilibrium_price, equilibrium_qty);
        Assert.IsTrue(Mathf.Abs(12083.3333f - xSquaredCurve.calculateAreaUnderCurve(100,50))<=0.01);

        equilibrium_price = 50;
        equilibrium_qty = 100;
        price_elasticity = -1;

        Curve straightCurve = Curve.createCurveByName("StraightCurve", price_elasticity, x_displacement, equilibrium_price, equilibrium_qty);
        Assert.IsTrue(Mathf.Abs(3750 - straightCurve.calculateAreaUnderCurve(100,50))<=0.01);
    }

    [Test]
    public void MarketScriptTest()
    {
        Debug.Log(string.Format("Iron Price:{0}, Iron Quantity:{1}", 
            marketscript.getPrice("Iron", 1),
            marketscript.getQuantity("Iron")));
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
