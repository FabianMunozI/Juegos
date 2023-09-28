using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoloGenerator : MonoBehaviour {

	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	[SerializeField] private GameObject[] montanias;
	[SerializeField] private GameObject[] trees;
	Transform parent;

	public int colliderLODIndex;
	public LODInfo[] detailLevels;

	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;
	public TextureData textureSettings;

	public Transform viewer;
	public Material mapMaterial;

	Vector2 viewerPosition;
	Vector2 viewerPositionOld;

	float meshWorldSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

	void Start() {

		textureSettings.ApplyToMaterial (mapMaterial);
		textureSettings.UpdateMeshHeights (mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

		float maxViewDst = detailLevels [detailLevels.Length - 1].visibleDstThreshold;
		meshWorldSize = meshSettings.meshWorldSize;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshWorldSize);

		UpdateVisibleChunks ();
		ColocarMontañas();
	}

	void Update() {
		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z);

		if (viewerPosition != viewerPositionOld) {
			foreach (TerrainChunk chunk in visibleTerrainChunks) {
				chunk.UpdateCollisionMesh ();
			}
		}

		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks ();
		}
	}
		
	void UpdateVisibleChunks() {
		HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2> ();
		for (int i = visibleTerrainChunks.Count-1; i >= 0; i--) {
			alreadyUpdatedChunkCoords.Add (visibleTerrainChunks[i].coord);
			visibleTerrainChunks[i].UpdateTerrainChunk();
		}
			
		int currentChunkCoordX = Mathf.RoundToInt (viewerPosition.x / meshWorldSize);
		int currentChunkCoordY = Mathf.RoundToInt (viewerPosition.y / meshWorldSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
				if (!alreadyUpdatedChunkCoords.Contains (viewedChunkCoord)) {
					if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
						terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					} else {
						TerrainChunk newChunk = new TerrainChunk (viewedChunkCoord,heightMapSettings,meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial);
						terrainChunkDictionary.Add (viewedChunkCoord, newChunk);
						newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
						newChunk.Load ();
					}
				}

			}
		}
	}

	void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible) {
		if (isVisible) {
			visibleTerrainChunks.Add (chunk);
		} else {
			visibleTerrainChunks.Remove (chunk);
		}
	}

	private void ColocarMontañas()
    {
		List<Vector3> montains = new List<Vector3>();

		montains.Add(new Vector3(1400, -.5f, 900));
		montains.Add(new Vector3(-900, -.5f, 1400));
		montains.Add(new Vector3(-1400, -.5f, -900));
		montains.Add(new Vector3(900, -.5f, -1400));
		

		List<Quaternion> mounts = new List<Quaternion>();

		mounts.Add(new Quaternion());
		mounts.Add(Quaternion.Euler(0, 270, 0));
		mounts.Add(Quaternion.Euler(0, 180, 0));
		mounts.Add(Quaternion.Euler(0, 90, 0));

		int index1, index2, aux;
		do
		{
			index1 = Random.Range(0, 4);
			index2 = Random.Range(0, 4);
		} while (index1 == index2);

		
		
		//Selección tipo de montaña
		aux = Random.Range(0, 3);

		//Pruebas
		aux = 1;

		if (aux == 0)
        {
			if (index1 == 0)
            {
				CrearIndicador(montains[0], montanias[0], mounts[0]);
			}
			else if (index1 == 1)
            {
				CrearIndicador(montains[1], montanias[0], mounts[1]);
			}
			else if (index1 == 2)
            {
				CrearIndicador(montains[2], montanias[0], mounts[2]);
			}
			else if (index1 == 3)
            {
				CrearIndicador(montains[3], montanias[0], mounts[3]);
			}
            else
            {
				Debug.Log("What?");
			}

			//CrearIndicador(montains[0], montanias[0], mounts[0]);
			//CrearIndicador(montains[1], montanias[0], mounts[1]);
			//CrearIndicador(montains[2], montanias[0], mounts[2]);
			//CrearIndicador(montains[3], montanias[0], mounts[3]);
		}
		else if (aux == 1)
        {
			if (index1 == 0)
			{
				CrearIndicador(new Vector3(1470, -.5f, 1250), montanias[1], mounts[0]);
			}
			else if (index1 == 1)
			{
				CrearIndicador(new Vector3(-1250, -.5f, 1470), montanias[1], mounts[1]);
			}
			else if (index1 == 2)
			{
				CrearIndicador(new Vector3(-1470, -.5f, -1250), montanias[1], mounts[2]);
			}
			else if (index1 == 3)
			{
				CrearIndicador(new Vector3(1250, -.5f, -1470), montanias[1], mounts[3]);
			}
			else
			{
				Debug.Log("What?");
			}

			//CrearIndicador(new Vector3(2900, -.5f, 2550), montanias[1], mounts[0]);
			//CrearIndicador(new Vector3(-2550, -.5f, 2900), montanias[1], mounts[1]);
			//CrearIndicador(new Vector3(-2900, -.5f, -2550), montanias[1], mounts[2]);
			//CrearIndicador(new Vector3(2550, -.5f, -2900), montanias[1], mounts[3]);
		}
		else if (aux == 2)
        {
			if (index1 == 0)
			{
				CrearIndicador(montains[0], montanias[2], mounts[0]);
			}
			else if (index1 == 1)
			{
				CrearIndicador(montains[1], montanias[2], mounts[1]);
			}
			else if (index1 == 2)
			{
				CrearIndicador(montains[2], montanias[2], mounts[2]);
			}
			else if (index1 == 3)
			{
				CrearIndicador(montains[3], montanias[2], mounts[3]);
			}
			else
			{
				Debug.Log("What?");
			}

			//CrearIndicador(montains[0], montanias[2], mounts[0]);
			//CrearIndicador(montains[1], montanias[2], mounts[1]);
			//CrearIndicador(montains[2], montanias[2], mounts[2]);
			//CrearIndicador(montains[3], montanias[2], mounts[3]);
		}
		else
        {
			Debug.Log("haber, pero qué pasó");
        }

		
		aux = Random.Range(0, 3);

		if (aux == 0)
		{
			if (index2 == 0)
			{
				CrearIndicador(montains[0], montanias[0], mounts[0]);
			}
			else if (index2 == 1)
			{
				CrearIndicador(montains[1], montanias[0], mounts[1]);
			}
			else if (index2 == 2)
			{
				CrearIndicador(montains[2], montanias[0], mounts[2]);
			}
			else if (index2 == 3)
			{
				CrearIndicador(montains[3], montanias[0], mounts[3]);
			}
			else
			{
				Debug.Log("What?");
			}
		}
		else if (aux == 1)
		{
			if (index2 == 0)
			{
				CrearIndicador(new Vector3(1470, -.5f, 1250), montanias[1], mounts[0]);
			}
			else if (index2 == 1)
			{
				CrearIndicador(new Vector3(-1250, -.5f, 1470), montanias[1], mounts[1]);
			}
			else if (index2 == 2)
			{
				CrearIndicador(new Vector3(-1470, -.5f, -1250), montanias[1], mounts[2]);
			}
			else if (index2 == 3)
			{
				CrearIndicador(new Vector3(1250, -.5f, -1470), montanias[1], mounts[3]);
			}
			else
			{
				Debug.Log("What?");
			}
		}
		else if (aux == 2)
		{
			if (index2 == 0)
			{
				CrearIndicador(montains[0], montanias[0], mounts[0]);
			}
			else if (index2 == 1)
			{
				CrearIndicador(montains[1], montanias[0], mounts[1]);
			}
			else if (index2 == 2)
			{
				CrearIndicador(montains[2], montanias[0], mounts[2]);
			}
			else if (index2 == 3)
			{
				CrearIndicador(montains[3], montanias[0], mounts[3]);
			}
			else
			{
				Debug.Log("What?");
			}
		}
		else
		{
			Debug.Log("haber, pero qué pasó 2");
		}		
		
	}

	private void CrearIndicador(Vector3 position, GameObject prefab, Quaternion rotation = new Quaternion())
	{
		var placePosition = position + new Vector3(.5f, .5f, .5f);
		var element = Instantiate(prefab, placePosition, rotation);
		element.transform.parent = parent;
		//diccionarioObstaculos.Add(position, element);
	}
}
