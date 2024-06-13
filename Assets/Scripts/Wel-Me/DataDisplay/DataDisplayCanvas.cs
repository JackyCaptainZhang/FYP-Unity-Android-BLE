using UnityEngine;
using System.Collections.Generic;
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine.Android;
using System.Text;
using UnityEngine.UI;

public class DataDisplayCanvas : MonoBehaviour
{
    public RawImage chartImage;
    public int maxDataPoints = 100;
    public Color lineColor = Color.red;  // Red lines
    public Color backgroundColor = Color.white;  // White background
    private Queue<float> dataQueue = new Queue<float>();
    private Texture2D texture;
    private int width = 500;
    private int height = 200;


    void Start()
    {
        // Initialize data queue
        for (int i = 0; i < maxDataPoints; i++)
        {
            dataQueue.Enqueue(0);
        }

        // Texture
        texture = new Texture2D(width, height);
        chartImage.texture = texture;
    }

    void Update()
    {
        // Get the data stream and update the data queue
        float newData = BleDataStorage.Float1;
            if (dataQueue.Count >= maxDataPoints)
            {
                dataQueue.Dequeue();
            }
            dataQueue.Enqueue(newData);

            // Plot
            DrawChart();
        
    }


    void DrawChart()
    {
        Color[] backgroundColors = new Color[width * height];
        for (int i = 0; i < backgroundColors.Length; i++)
        {
            backgroundColors[i] = backgroundColor;
        }
        texture.SetPixels(backgroundColors);

        float[] dataArray = dataQueue.ToArray();

        // Calculate max and min
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;
        foreach (float value in dataArray)
        {
            if (value < minValue) minValue = value;
            if (value > maxValue) maxValue = value;
        }

        // Draw the line
        for (int i = 0; i < dataArray.Length - 1; i++)
        {
            int x0 = i * (width / maxDataPoints);
            int x1 = (i + 1) * (width / maxDataPoints);

            float normalizedValue0 = (dataArray[i] - minValue) / (maxValue - minValue);  // Normalize data to 0-1
            float normalizedValue1 = (dataArray[i + 1] - minValue) / (maxValue - minValue);

            int y0 = Mathf.FloorToInt(normalizedValue0 * height);
            int y1 = Mathf.FloorToInt(normalizedValue1 * height);

            DrawLine(texture, x0, y0, x1, y1, lineColor);
        }
        texture.Apply();
    }

    void DrawLine(Texture2D texture, int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            texture.SetPixel(x0, y0, color);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
}
