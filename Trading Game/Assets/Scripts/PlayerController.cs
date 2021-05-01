using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Player player = new Player();
    StringBuilder sb = new StringBuilder();

    private string town = "Luebeck";

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnGUI()
    {
        GUILayout.Box($"Player Gold:{player.gold}");
        displayPlayerResources();
        GUILayout.Box($"Selected Town:{town}");
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

    private void displayPlayerResources()
    {
        if (player.inventories.Keys.Count > 0)
        {
            sb.Clear();
            sb.Append("Player Resources:\n");

            foreach (var resource in player.inventories.Keys)
            {
                sb.AppendFormat("{0}: {1}\n", resource, player.inventories[resource].quantity);
            }

            GUILayout.Box(sb.ToString());
        }
    }
}
