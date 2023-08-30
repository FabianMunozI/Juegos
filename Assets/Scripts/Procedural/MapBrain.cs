using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;


namespace SVS.ChessMaze
{

    public class MapBrain : MonoBehaviour
    {
        //parametros de algoritmo gentico
        [SerializeField, Range(20,100)]
        private int populationSize = 20;

        [SerializeField, Range(0,100)]
        private int crossoverRate = 100;

        private double crossoverRatePercent;
        
        [SerializeField, Range(0,100)]
        private int mutacionRate = 0;

        private double mutacionRatePercent;

        [SerializeField, Range(1,100)]
        private int limiteGeneracion = 10;


        //varibles del algoritmo

        private List<MapaCandidato> currentGeneracion;
        private int totalFitnessThis, bestFitnessAll = 0;
        private MapaCandidato bestMap = null;
        private int bestMapGenerationNumber = 0, generationNumber = 1;

        //Fitness parametros

        [SerializeField]
        private int fitnessCornerMin = 6, fitnessCornerMax = 9;
        
        [SerializeField, Range(1,3)]
        private int fitnessCornerPeso = 1, fitnessNearCornerPeso = 1;

        [SerializeField, Range(1,5)]
        private int fitnessPathPeso = 1;

        [SerializeField, Range(0.3f,1f)]
        private float fitnessObstPeso = 1;

        //parametros inicio mapa

        [SerializeField, Range(3,20)]
        private int anchoMapa = 11, largoMapa = 11;
        private Vector3 startPosition, exitPosition;
        private GrillaMapa grilla;

        //RIO
        private Vector3 inicioRio, finalRio;

        //Visualizar grilla

        public VizualisadorMapa vizualisadorMapa;
        DateTime startDate ,endDate;
        private bool isAlgRun = false;

        public bool IsAlgRun {get => isAlgRun;}

        //metodos

        private void Start()
        {
            mutacionRatePercent = mutacionRate / 100D;
            crossoverRatePercent = crossoverRate /100D;
        }

        public void RunAlgoritmo()
        {
            UiController.instance.ResetScreen();

            ResetAlgoVariab();

            vizualisadorMapa.LimpiarMapa();

            /*

            grilla = new GrillaMapa(anchoMapa, largoMapa);

            MapHelper.RandomChooseStartExit(grilla, ref startPosition, ref exitPosition);

            // RIO
            MapHelper.RandomChooseStartExit(grilla, ref inicioRio, ref finalRio, startPosition, exitPosition, true);

            isAlgRun = true;
            startDate = DateTime.Now;
            FindOptimalSolution(grilla);

            */
            
        }

        private void FindOptimalSolution(GrillaMapa grilla)
        {
            currentGeneracion = new List<MapaCandidato>(populationSize);
            Debug.Log("Pop Size: " + populationSize);
            for (int k = 0; k < grilla.Ancho; k++)
            {
                for (int j = 0; j < grilla.Largo; j++)
                {
                    if (grilla.GetCelda(k, j).TipoObjeto == TipoObjCelda.InicioRio)
                    {
                        Debug.Log("PORONGA1");
                    }
                }
            }



            //for (int i = 0; i < populationSize; i++)
            //{
                //Debug.Log("Candidate Pop " + i);
                Debug.Log("Ancho: " + grilla.Ancho);
                Debug.Log("Largo: " + grilla.Largo);
                //var candidateMap = new MapaCandidato(grilla);
                MapaCandidato candidateMap = new MapaCandidato(grilla);
                for (int k = 0; k < candidateMap.Grilla.Ancho; k++)
                {
                    for (int j = 0; j < candidateMap.Grilla.Largo; j++)
                    {
                        if (candidateMap.Grilla.GetCelda(k, j).TipoObjeto == TipoObjCelda.InicioRio)
                        {
                            Debug.Log("Pre-PORONGA2");
                        }
                    }
                }

                candidateMap.CrearMapa(startPosition, exitPosition, inicioRio, finalRio, true); 

                for(int k=0; k < candidateMap.Grilla.Ancho; k++)
                {
                    for(int j=0; j < candidateMap.Grilla.Largo; j++)
                    {
                        if(candidateMap.Grilla.GetCelda(k,j).TipoObjeto == TipoObjCelda.InicioRio)
                        {
                            Debug.Log("PORONGA2");
                        }
                    }
                }                

                currentGeneracion.Add(candidateMap);
            //}
            
            StartCoroutine(GeneticAlgorithm());

        }

        private void ResetAlgoVariab() //! Revisar despues
        {
            totalFitnessThis = 0;
            bestFitnessAll = 0;
            bestMap = null;
            bestMapGenerationNumber = 0;
            generationNumber = 0;
        }

        private IEnumerator GeneticAlgorithm()
        {
            totalFitnessThis = 0;
            int bestFitnessScoreThis = 0 ;
            MapaCandidato bestMapThis = null;
            foreach (var candidate in currentGeneracion)
            {
                candidate.FindPath();
                candidate.GenerarRio();
                candidate.Reparar();
                var fitness = CalcularFitness(candidate.ReturnMapData());

                totalFitnessThis += fitness;
                if (fitness > bestFitnessScoreThis)
                {
                    bestFitnessScoreThis = fitness;
                    bestMapThis = candidate;
                }
            }
            if (bestFitnessScoreThis > bestFitnessAll)
            {
                bestFitnessAll = bestFitnessScoreThis;
                bestMap = bestMapThis.DeepClone();
                bestMapGenerationNumber = generationNumber;
            }

            generationNumber++;
            yield return new WaitForEndOfFrame();

            UiController.instance.SetLoadValue(generationNumber / (float)limiteGeneracion);

            //Debug.Log("Current generation" + generationNumber + " score: " + bestMapThis);

            if (generationNumber < limiteGeneracion)
            {
                List<MapaCandidato> nextGeneration = new List<MapaCandidato>();

                while (nextGeneration.Count < populationSize)
                {
                    var padre1 = currentGeneracion[RouletteWheelSelection()];
                    var padre2 = currentGeneracion[RouletteWheelSelection()];

                    MapaCandidato hijo1, hijo2;

                    CrossOverPadres(padre1, padre2, out hijo1, out hijo2);

                    hijo1.AddMutacion(mutacionRatePercent);
                    hijo2.AddMutacion(mutacionRatePercent);

                    nextGeneration.Add(hijo1);
                    nextGeneration.Add(hijo2);
                }
                currentGeneracion = nextGeneration;

                StartCoroutine(GeneticAlgorithm());
            }
            else
            {
                ShowResults();
            }
        }

        private void ShowResults()
        {
            isAlgRun = false;
            //Debug.Log("Best solucion at generacion " + bestMapGenerationNumber + " con puntaje " + bestFitnessAll);

            var data = bestMap.ReturnMapData();
            vizualisadorMapa.VisualizarMapa(bestMap.Grilla, data);

            UiController.instance.HideLoadScreen();

            //Debug.Log("Path Largo:" + data.path);
            //Debug.Log("Corner cuenta:" + data.cornerList.Count);

            endDate = DateTime.Now;
            long elapsedTicks = endDate.Ticks - startDate. Ticks;
            TimeSpan elapSpan = new TimeSpan(elapsedTicks);
            //Debug.Log("Time, necesario correr optimo " + elapSpan.TotalSeconds);
        }

        private void CrossOverPadres(MapaCandidato padre1, MapaCandidato padre2, out MapaCandidato hijo1, out MapaCandidato hijo2)
        {
            hijo1 = padre1.DeepClone();
            hijo2 = padre2.DeepClone();

            if (Random.value < crossoverRatePercent)
            {
                int numBits = padre1.ObstaclesArray.Length;

                int crossOverIndex = Random.Range(0,numBits);

                for (int i = crossOverIndex ; i < numBits ; i++)
                {
                    hijo1.PlaceObstaculos(i, padre2.isObstacleAt(i));
                    hijo2.PlaceObstaculos(i, padre1.isObstacleAt(i));
                }
            }
        }


        private int RouletteWheelSelection()
        {
            int valorRandom = Random.Range(0,totalFitnessThis);
            for (int i = 0 ; i < populationSize ; i++)
            {
                valorRandom -= CalcularFitness(currentGeneracion[i].ReturnMapData());
                if (valorRandom <= 0)
                {
                    return i;
                }
            }
            return populationSize-1;
        }

        private int CalcularFitness(MapData mapData)
        {
            //que tan bueno es el mapa
            int numeroObst = mapData.obstaclesArray.Where(isObstacle => isObstacle).Count();
            int puntaje = mapData.path.Count * fitnessPathPeso + (int)(numeroObst * fitnessObstPeso);
            int cornerCount = mapData.cornerList.Count;
            if (cornerCount >= fitnessCornerMin && cornerCount <= fitnessCornerMax)
            {
                puntaje += cornerCount + fitnessCornerPeso;
            }else if (cornerCount > fitnessCornerMax)
            {
                puntaje -= fitnessCornerPeso * (cornerCount - fitnessCornerMax);
            }else if (cornerCount < fitnessCornerMin)
            {
                puntaje -= fitnessCornerPeso * fitnessCornerMin;
            }
            puntaje -= mapData.cornerNearEachOther * fitnessNearCornerPeso;
            return puntaje;
        }

    }
}





