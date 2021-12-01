using UnityEngine;
using System.Collections;
using System.IO;

// This class encapsulates all of the metrics that need to be tracked in your game. These may range
// from number of deaths, number of times the player uses a particular mechanic, or the total time
// spent in a level. These are unique to your game and need to be tailored specifically to the data
// you would like to collect. The examples below are just meant to illustrate one way to interact
// with this script and save data.
public class MetricManager : MonoBehaviour
{
    public static MetricManager instance;

    public int m_NumDeaths = 0;
    private int m_BubblesLandedOn = 0;
    private int m_ButterfliesUsed = 0;
    private int m_ButterflyDamages = 0;
    private int m_FallingAcidDamages = 0;
    private int m_WhiteBloodCellDamages = 0;
    private int m_EnemiesKilled = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddDeath()
    {
        m_NumDeaths++;
    }

    public void AddFallingAcid()
    {
        m_FallingAcidDamages++;
    }

    public void AddBubble()
    {
        m_BubblesLandedOn++;
    }

    public void AddButterflyBoost()
    {
        m_ButterfliesUsed++;
    }

    public void AddButterflyDamage()
    {
        m_ButterflyDamages++;
    }

    public void AddWhiteBloodCellDamage()
    {
        m_WhiteBloodCellDamages++;
    }

    public void AddConfirmedKill()
    {
        m_EnemiesKilled++;
    }

    // Converts all metrics tracked in this script to their string representation
    // so they look correct when printing to a file.
    private string ConvertMetricsToStringRepresentation()
    {
        string metrics = "Here are my metrics:\n";
        metrics += "Deaths: " + m_NumDeaths.ToString() + "\n";
        metrics += "Acid droplet injuries sustained: " + m_FallingAcidDamages.ToString() + "\n";
        metrics += "White blood cell attacks sustained: " + m_WhiteBloodCellDamages.ToString() + "\n";
        metrics += "Butterfly attacks sustained: " + m_ButterflyDamages.ToString() + "\n";
        metrics += "Enemies killed: " + m_EnemiesKilled.ToString() + "\n";
        metrics += "Bubbles landed on: " + m_BubblesLandedOn.ToString() + "\n";
        metrics += "Butterflies landed on: " + m_ButterfliesUsed.ToString() + "\n";
        return metrics;
    }

    // Uses the current date/time on this computer to create a uniquely named file,
    // preventing files from colliding and overwriting data.
    private string CreateUniqueFileName()
    {
        string dateTime = System.DateTime.Now.ToString();
        dateTime = dateTime.Replace("/", "_");
        dateTime = dateTime.Replace(":", "_");
        dateTime = dateTime.Replace(" ", "___");
        return "WoundedSoul_Metrics_" + dateTime + ".txt";
    }

    // Generate the report that will be saved out to a file.
    private void WriteMetricsToFile()
    {
        string totalReport = "Report generated on " + System.DateTime.Now + "\n\n";
        totalReport += "Total Report:\n";
        totalReport += ConvertMetricsToStringRepresentation();
        totalReport = totalReport.Replace("\n", System.Environment.NewLine);
        string reportFile = CreateUniqueFileName();

#if !UNITY_WEBPLAYER
        File.WriteAllText(reportFile, totalReport);
#endif
    }

    // The OnApplicationQuit function is a Unity-Specific function that gets
    // called right before your application actually exits. You can use this
    // to save information for the next time the game starts, or in our case
    // write the metrics out to a file.
    private void OnApplicationQuit()
    {
        WriteMetricsToFile();
    }
}
