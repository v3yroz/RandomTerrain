using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;
    [SerializeField] private int pixelSize = 10;

    [Header("Wartości Generatora")]
    [SerializeField] private int seed;
    [SerializeField] private int a;

    private Texture2D terrainTexture;
    private SpriteRenderer spriteRenderer;

    private const int m = 1 << 20; // m >= 2^20
    private int c = Mathf.FloorToInt((3 - Mathf.Sqrt(3)) / 6 * m);

    private void Start() {

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        GenerateTerrain();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.R)) {

            terrainTexture = null;
            GenerateTerrain();
        }
    }

    private void GenerateTerrain() {

        terrainTexture = new Texture2D(width, height);
        terrainTexture.filterMode = FilterMode.Point;

        int[,] terrainData = GenerateTerrainData();

        for (int x = 0; x < width; x++) {

            for (int y = 0; y < height; y++) {

                Color color = GetColorForValue(terrainData[x, y]);
                terrainTexture.SetPixel(x, y, color);
            }
        }

        terrainTexture.Apply();

        spriteRenderer.sprite = Sprite.Create(terrainTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f, pixelSize);
    }

    private int[,] GenerateTerrainData() {

        int[,] data = new int[width, height];
        int current = seed;
        int prevcurrent;

        for (int x = 0; x < width; x++) {

            for (int y = 0; y < height; y++) {
                
                prevcurrent = current;
                Debug.Log(current);
                current = (a * current + c) % m;
                Debug.Log("(" + a + " * " + prevcurrent + " + " + c + ") % " + m + " = " + current);
                float normalized = (float)current / m;
                Debug.Log(normalized);
                data[x, y] = Mathf.FloorToInt(normalized * 100);
                Debug.Log(data[x, y]);
            }
        }

        return data;
    }

    private Color GetColorForValue(int value) {

        if (value < 25) return Color.blue;
        if (value < 50) return Color.green;
        if (value < 75) return Color.red;
        return Color.yellow;
    }
}
