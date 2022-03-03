﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary> 
/// The main game manager
/// </summary> 
public class CelluloManager : MonoBehaviour
{
    public GameObject _celluloConnectionCanvas;
    public GameObject _celluloScanCanvas;

    public ScrollRect _cellulosListScrollRect;
    public GameObject _cellulosListPanel;
    public GameObject _cellulosListElementPanel;
    public GameObject _spawnedCellulos;

    // Dictionnary of the Cellulo Robots, with its id and GameObject
    public static Dictionary<int, GameObject> _celluloList = new Dictionary<int, GameObject>();
    // Dictionnary of the Cellulo Robots panel in list, with its id and GameObject
    public static Dictionary<int, GameObject> _celluloPanelList = new Dictionary<int, GameObject>();
    // Set of the ids of the Cellulo Robots that are connected
    public static HashSet<int> _connectedCellulos = new HashSet<int>();
    // Dictionnary of the Cellulo mac addresses to their number
    public static Dictionary<string, int> _celluloNumbers = new Dictionary<string, int>()
    {
        {"00:06:66:74:3A:82", 1},
        {"00:06:66:74:3E:82", 2},
        {"00:06:66:74:3E:93", 3},
        {"00:06:66:74:40:D3", 4},
        {"00:06:66:74:40:DB", 5},
        {"00:06:66:74:40:DC", 6},
        {"00:06:66:74:40:E3", 7},
        {"00:06:66:74:40:FF", 8},
        {"00:06:66:74:41:4C", 9},
        {"00:06:66:E7:8A:44", 10},
        {"00:06:66:E7:8A:CD", 11},
        {"00:06:66:E7:8A:CE", 12},
        {"00:06:66:E7:8A:D1", 13},
        {"00:06:66:E7:8A:D6", 14},
        {"00:06:66:E7:8A:D8", 15},
        {"00:06:66:E7:8A:D9", 16},
        {"00:06:66:E7:8A:DE", 17},
        {"00:06:66:E7:8A:E1", 18},
        {"00:06:66:E7:8A:E5", 19},
        {"00:06:66:E7:8A:E6", 20},
        {"00:06:66:E7:8E:59", 21},
        {"00:06:66:E7:8E:64", 22},
        {"00:06:66:D2:CF:7B", 23},
        {"00:06:66:D2:CF:82", 24},
        {"00:06:66:D2:CF:83", 25},
        {"00:06:66:D2:CF:85", 26},
        {"00:06:66:D2:CF:91", 27},
        {"00:06:66:D2:CF:8B", 28},
        {"00:06:66:D2:CF:97", 29}
    };

    // The cellulo mac address selected in the scanner view
    public static string _selectedCelluloToConnectTo = null;
    // The id of the cellulo agent which has the local view
    private int _localView = 0;
		
	/// <summary>
	/// Set the environment variable at start and Initialize the library and scanner at start.
	/// </summary>
    void Awake()
    {
        Environment.SetEnvironmentVariable("BLUETOOTH_FORCE_DBUS_LE_VERSION","1");
        Environment.SetEnvironmentVariable("QT_EVENT_DISPATCHER_CORE_FOUNDATION","1");
#if !UNITY_WEBGL 
        try
        {
            Cellulo.initialize();
            
        }
        catch (Exception e)
        {
            Debug.Log("error initializing lib: " + e.Message);
        }
#endif
    }

    private void Start()
    {
        if (_spawnedCellulos != null)
        {
            for (int i = 0; i < _spawnedCellulos.transform.childCount; i++)
            {
                if (_spawnedCellulos.transform.GetChild(i).gameObject.activeSelf) 
                    AddAgent(_spawnedCellulos.transform.GetChild(i).GetComponent<CelluloAgent>());
            }
        }
    }

    /// <summary>
    /// Check user inputs at each frame.
    /// </summary>
    void Update()
    {
        // Application quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SettingsCanvasControls();
        }
    }

	/// <summary>
	/// Displays and hides the cellulo connection and cellulo scanner canvas
	/// </summary>
    public void SettingsCanvasControls()
    {
        if (_celluloConnectionCanvas.activeSelf && _celluloScanCanvas.activeSelf)
        {
            _celluloConnectionCanvas.SetActive(false);
            _celluloScanCanvas.SetActive(false);
        }
        else
        {
            _celluloConnectionCanvas.SetActive(true);
            _celluloScanCanvas.SetActive(true);
        }
    }

	/// <summary>
	/// Getter for the current cellulo id with the local view
	/// </summary>
	/// <returns>
	/// The current cellulo id with the local view
	/// </returns>
    public int GetCurrentLocalView()
    {
        return _localView;
    }

    public void AddAgent(CelluloAgent agent)
    {
        _celluloList.Add(agent.agentID, agent.gameObject);
        // Create and configure the Cellulo panel and add it to the list
        GameObject newCelluloListElementPanel = Instantiate(_cellulosListElementPanel, Vector3.zero, Quaternion.identity) as GameObject;
        newCelluloListElementPanel.transform.SetParent(_cellulosListPanel.transform, false);
        Toggle _toggle = newCelluloListElementPanel.GetComponentInChildren<Toggle>();
        _toggle.group = _cellulosListPanel.GetComponent<ToggleGroup>();

        _celluloPanelList.Add(agent.agentID, newCelluloListElementPanel);
        CelluloListElement celluloListElement = newCelluloListElementPanel.GetComponent<CelluloListElement>();
        celluloListElement.SetId(agent.agentID);
        celluloListElement.ResetPanel(ConnectionStatus.ConnectionStatusDisconnected);

        // Set the scrollbar position to the top
        Canvas.ForceUpdateCanvases();
        _cellulosListScrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();

    }
    /// <summary>
    /// Deletes an agent with the given id.
    /// </summary>
    /// <param name="id">
    /// The id of the agent to delete.
    /// </param>
    public void RemoveAgent(int id)
    {
        // Disconnect the module if necessary
        _celluloList[id].GetComponent<CelluloConnection>().DisconnectFromCelluloRobot();

        // Destroy the cellulo robot Gameobject and remove it from the list
        Destroy(_celluloList[id]);
        _celluloList.Remove(id);

        // Destroy the cellulo robot panel and remove it from the list
        Destroy(_celluloPanelList[id]);
        _celluloPanelList.Remove(id);
    }
}
