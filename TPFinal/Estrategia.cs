
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using tp1;

namespace tpfinal
{

	public class Estrategia
	{
	
		public String Consulta1(List<string> datos)
		{
            //Listas de heap y otro metodo
			List<Dato> heap = new List<Dato>();
			List<Dato> otro = new List<Dato>();

            //Metodo heap, se guarda el tiempo actual luego se resta el tiempo de fin. Se hace lo mismo con buscar con otro.
			DateTime inicioHeap = DateTime.Now;
			BuscarConHeap(datos, 5, heap);
			DateTime finHeap = DateTime.Now;
			double tiempoHeap = (finHeap - inicioHeap).TotalMilliseconds;

			DateTime inicioOtro = DateTime.Now;
			BuscarConOtro(datos, 5, otro);
			DateTime finOtro = DateTime.Now;
			double tiempoOtro = (finOtro - inicioOtro).TotalMilliseconds;

			return "Tiempo BuscarConHeap: " + tiempoHeap + " ms\nTiempo BuscarConOtro: " + tiempoOtro + " ms";

		}


		public String Consulta2(List<string> datos)
		{
            //Se crea una heap y una lista de camino luego se llama al metodo buscarConHeap
			List<Dato> heap = new List<Dato>();
			BuscarConHeap(datos, datos.Count, heap); 
			List<string> camino = new List<string>();

            //Si la heap no esta vacia agrega el hijo mas a la izquierda a la lista camino
			if (heap.Count > 0)
			{
				camino.Add(heap[0].texto); 
				int index = 0;
				while (2 * index + 1 < heap.Count) 
				{
					index = 2 * index + 1;
					camino.Add(heap[index].texto);
				}
			}

            //Imprime el camino
            string resultado = "Camino a la hoja más izquierda: ";
            for (int i = 0; i < camino.Count; i++)
            {
                if (i > 0)
                {
                    resultado += " -> ";
                }
                resultado += camino[i];
            }

            return resultado;

        }



		public String Consulta3(List<string> datos)
		{
            //Se crea una heap, una cola y se llama al metodo buscarConHeap
			List<Dato> heap = new List<Dato>();
			BuscarConHeap(datos, datos.Count, heap); 
			Cola<(Dato, int)> cola = new Cola<(Dato, int)>();
            cola.encolar((heap[0], 0));
			int nivelActual = 0;

            string resultado = "Niveles de la Heap:\nNivel 0: ";

            //Mientras la cola no este vacia va ir imprimiendo y buscando elementos por niveles en la heap.
            while (!cola.esVacia())
            {
                (Dato dato,int nivel) = cola.desencolar();

                if (nivel != nivelActual)
                {
                    resultado += "\nNivel " + nivel + ": ";
                    nivelActual = nivel;
                }
                else if (nivel > 0)
                {
                    resultado += ", ";
                }
                resultado += dato.texto;

                int tamaño = heap.IndexOf(dato);
                int tamIzq = 2 * tamaño + 1;
                int tamDer = 2 * tamaño + 2;

                if (tamIzq < heap.Count)
                {
                    cola.encolar((heap[tamIzq], nivel + 1));
                }
                if (tamDer < heap.Count)
                {
                    cola.encolar((heap[tamDer], nivel + 1));
                }
            }

            return resultado;

        }


		public void BuscarConOtro(List<string> datos, int cantidad, List<Dato> collected)
        {
            List<Dato> listaDatos = new List<Dato>(); //Se crea la lista
            foreach (var texto in datos)
            {
                bool encontrado = false;
                for (int i = 0; i < listaDatos.Count; i++)
                {
                    if (listaDatos[i].texto == texto)
                    {
                        listaDatos[i].ocurrencia++;
                        encontrado = true;
                        break;
                    }
                }
                if (!encontrado)
                {
                    listaDatos.Add(new Dato(1, texto));
                }
            }

            // Ordenar la lista de mayor a menor ocurrencias
            for (int i = 0; i < listaDatos.Count - 1; i++)
            {
                for (int j = 0; j < listaDatos.Count - i - 1; j++)
                {
                    if (listaDatos[j].ocurrencia < listaDatos[j + 1].ocurrencia)
                    {
                        Dato temp = listaDatos[j];
                        listaDatos[j] = listaDatos[j + 1];
                        listaDatos[j + 1] = temp;
                    }
                }
            }

            for (int i = 0; i < cantidad && i < listaDatos.Count; i++)
            {
                collected.Add(listaDatos[i]);
            }

        }


		public void BuscarConHeap(List<string> datos, int cantidad, List<Dato> collected)
        {
            List<Dato> heap = new List<Dato>(); //Creacion de la Heap

            foreach (var texto in datos) 
            {
                bool encontrado = false;
                for (int i = 0; i < heap.Count; i++)
                {
                    if (heap[i].texto == texto)
                    {
                        heap[i].ocurrencia++;
                        encontrado = true;
                        break;
                    }
                }
                if (!encontrado)
                {
                    heap.Add(new Dato(1, texto));
                }
            }

            for (int i = (heap.Count / 2) - 1; i >= 0; i--) //for para construir la heap
            {
                int index = i;
                while (true)
                {
                    int tamaño = index;
                    int hijoIzq = 2 * index + 1;
                    int hijoDer = 2 * index + 2;

                    if (hijoIzq < heap.Count && heap[hijoIzq].ocurrencia > heap[tamaño].ocurrencia)
                    {
                        tamaño = hijoIzq;
                    }

                    if (hijoDer < heap.Count && heap[hijoDer].ocurrencia > heap[tamaño].ocurrencia)
                    {
                        tamaño = hijoDer;
                    }

                    if (tamaño != index)
                    {
                        Dato temp = heap[index];
                        heap[index] = heap[tamaño];
                        heap[tamaño] = temp;
                        index = tamaño;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = 0; i < cantidad && heap.Count > 0; i++) //for para extraer los elementos con mayor ocurrencia
            {
                collected.Add(heap[0]);

                // Mover el último elemento al tope y reducir el tamaño de la heap
                heap[0] = heap[heap.Count - 1];
                heap.RemoveAt(heap.Count - 1);

                // Restaurar la propiedad de la heap
                int index = 0;
                while (true)
                {
                    int tamaño = index;
                    int hijoIzq = 2 * index + 1;
                    int hijoDer = 2 * index + 2;

                    if (hijoIzq < heap.Count && heap[hijoIzq].ocurrencia > heap[tamaño].ocurrencia)
                    {
                        tamaño = hijoIzq;
                    }

                    if (hijoDer < heap.Count && heap[hijoDer].ocurrencia > heap[tamaño].ocurrencia)
                    {
                        tamaño = hijoDer;
                    }

                    if (tamaño != index)
                    {
                        Dato temp = heap[index];
                        heap[index] = heap[tamaño];
                        heap[tamaño] = temp;
                        index = tamaño;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
	}
}