using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public struct ciudad
{
    public GameObject nombre;
    public int ID;
    public string city;

}
 
public class busqueda : MonoBehaviour
{
    //materiales
    public Material MInicio;
    public Material MMedio;
    public Material MFin;
    public Material Mreset;
    
    public bool mat;
    public Dropdown DDInicio;
    public Dropdown DDfinal;
    public Dropdown DDintegrantes;

    public GameObject texto;
    public GameObject textoF;

    public GameObject btnRutacorta;

    private CGrafo mapita;

    public int inicio, final;//seleecion del usuario
    int dist = 0;//distancias entre nodos para camino mas corto
    int n = 0, m = 0;//auxiliares de recorrido
    int cantNodos; //Numero de ciudades
    string dato="";//captura el dato
    int prof = 50;//nivel de profundidad de busqueda
    int cont = 0;//contdor de profundidad

    List<int> rutaF = new List<int>();//Ruta final
    List<int> rutaC = new List<int>();//Ruta final
    int[,] mapa = new int[32,32]
        {
      {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0 },
      {0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0 },
      {0,1,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0 },
      {0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0 },
      {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0 },
      {0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
      {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0 },
      {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1,0,0,0,0,0,0,0,0,0,0 },
      {0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0 },
      {0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0 },
      {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
      {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0 },
      {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0 },
      {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,1,0,0 },
      {0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0 },
      {0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0 },
      {0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      {0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
      };
    public bool[] visited = new bool[32];
    private bool encontrado = false;

    //definir ciudades
    public ciudad riohacha,santamarta,valledupar,barranquilla, cartagena, sincelejo, monteria, cucuta, bucaramanga,arauca,puertocarreño,medellin,tunja,quibdo,manizales,
        pereira,armenia,ibague,cali,neiva,popayan,florencia,mocoa,pasto,bogota,villavicencio, yopal, inirida, guaviare,mitu,leticia;
    public ciudad[] ciudades=new  ciudad[30];
    public string []integrantes= new string[4]{"Jessica Arias", "Karen Saavedra", "Diego Suarez", "Nicolas Torres"};
    
    // Start is called before the first frame update
    void Start()
    {
        mapita=GetComponent<CGrafo>();
        btnRutacorta.SetActive(false);
        //buscar ciudades G.O. y agruparlos
        ciudades[0].city="Seleccione...";
        ciudades[1].nombre=GameObject.Find("Riohacha");
        riohacha.nombre=GameObject.Find("Riohacha");
        ciudades[1].ID=17;
        riohacha.city="Riohacha";
        ciudades[1].city="Rioacha";

        ciudades[2].nombre=GameObject.Find("SantaMarta");
        santamarta.nombre=GameObject.Find("SantaMarta");
        ciudades[2].ID=18;
        ciudades[2].city="Santa Marta";

        ciudades[3].nombre=GameObject.Find("Valledupar");
        valledupar.nombre=GameObject.Find("Valledupar");
         ciudades[3].ID=10;
        ciudades[3].city="Valledupar";

        ciudades[4].nombre=GameObject.Find("Barranquilla");
        barranquilla.nombre=GameObject.Find("Barranquilla");
        ciudades[4].ID=3;
        ciudades[4].city="Barranquilla";

        ciudades[5].nombre=GameObject.Find("Cartagena");
        cartagena.nombre=GameObject.Find("Cartagena");
        ciudades[5].ID=4;
        ciudades[5].city="Cartagena";

        ciudades[6].nombre=GameObject.Find("Sincelejo");
        sincelejo.nombre=GameObject.Find("Sincelejo");
        ciudades[6].ID=27;
        ciudades[6].city="Sincelejo";

        ciudades[7].nombre=GameObject.Find("Monteria");
        monteria.nombre=GameObject.Find("Monteria");
        ciudades[7].ID=12;
        ciudades[7].city="Monteria";

        ciudades[8].nombre=GameObject.Find("Cucuta");
        cucuta.nombre=GameObject.Find("Cucuta");
        ciudades[8].ID=21;
        ciudades[8].city="Cucuta";

        ciudades[9].nombre=GameObject.Find("Bucaramanga");
        bucaramanga.nombre=GameObject.Find("Bucaramanga");
        ciudades[9].ID=26;
        ciudades[9].city="Bucaramanga";
        
        ciudades[10].nombre=GameObject.Find("Arauca");
        arauca.nombre=GameObject.Find("Arauca");
        ciudades[10].ID=2;
        ciudades[10].city="Arauca";

        ciudades[11].nombre=GameObject.Find("PuertoCarreño");
        puertocarreño.nombre=GameObject.Find("PuertoCarreño");
         ciudades[11].ID=31;
        ciudades[11].city="Puerto Carreño";

        ciudades[12].nombre=GameObject.Find("Medellin");
        medellin.nombre=GameObject.Find("Medellin");
        ciudades[12].ID=1;
        ciudades[12].city="Medellin";

        ciudades[13].nombre=GameObject.Find("Tunja");
        tunja.nombre=GameObject.Find("Tunja");
        ciudades[13].ID=5;
        ciudades[13].city="Tunja";

        ciudades[14].nombre=GameObject.Find("Quibdo");
        quibdo.nombre=GameObject.Find("Quibdo");
        ciudades[14].ID=11;
        ciudades[14].city="Quibdo";

        ciudades[15].nombre=GameObject.Find("Manizales");
        manizales.nombre=GameObject.Find("Manizales");
        ciudades[15].ID=6;
        ciudades[15].city="Manizales";

        ciudades[16].nombre=GameObject.Find("Pereira");
        pereira.nombre=GameObject.Find("Pereira");
        ciudades[16].ID=24;
        ciudades[16].city="Pereira";

        ciudades[17].nombre=GameObject.Find("Armenia");
        armenia.nombre=GameObject.Find("Armenia");
        ciudades[17].ID=23;
        ciudades[17].city="Armenia";

        ciudades[18].nombre=GameObject.Find("Ibague");
        ibague.nombre=GameObject.Find("Ibague");
        ciudades[18].ID=28;
        ciudades[18].city="Ibague";

        ciudades[19].nombre=GameObject.Find("Cali");
        cali.nombre=GameObject.Find("Cali");
        ciudades[19].ID=29;
        ciudades[19].city="Cali";

        ciudades[20].nombre=GameObject.Find("Neiva");
        neiva.nombre=GameObject.Find("Neiva");
        ciudades[20].ID=16;
        ciudades[20].city="Neiva";

        ciudades[21].nombre=GameObject.Find("Popayan");
        popayan.nombre=GameObject.Find("Popayan");
        ciudades[21].ID=9;
        ciudades[21].city="Popayan";

        ciudades[22].nombre=GameObject.Find("Florencia");
        florencia.nombre=GameObject.Find("Florencia");
        ciudades[22].ID=7;
        ciudades[22].city="Florencia";

        ciudades[23].nombre=GameObject.Find("Mocoa");
        mocoa.nombre=GameObject.Find("Mocoa");
        ciudades[23].ID=22;
        ciudades[23].city="Mocoa";

        ciudades[24].nombre=GameObject.Find("Pasto");
        pasto.nombre=GameObject.Find("Pasto");
        ciudades[24].ID=20;
        ciudades[24].city="Pasto";

        ciudades[25].nombre=GameObject.Find("Bogota");
        bogota.nombre=GameObject.Find("Bogota");
        ciudades[25].ID=13;
        ciudades[25].city="Bogota";
        
        ciudades[26].nombre=GameObject.Find("Villavicencio");
        villavicencio.nombre=GameObject.Find("Villavicencio");
        ciudades[26].ID=19;
        ciudades[26].city="Villavicencio";

        ciudades[27].nombre=GameObject.Find("Yopal");
        yopal.nombre=GameObject.Find("Yopal");
        ciudades[27].ID=8;
        ciudades[27].city="Yopal";

        ciudades[28].nombre=GameObject.Find("Inirida");
        inirida.nombre=GameObject.Find("Inirida");
         ciudades[28].ID=14;
        ciudades[28].city="Inirida";

        ciudades[29].nombre=GameObject.Find("Guaviare");
        guaviare.nombre=GameObject.Find("Guaviare");
        ciudades[29].ID=15;
        ciudades[29].city="Guaviare";
        cantNodos=32;
        /*
        ciudades[29].nombre=GameObject.Find("Mitu");
        mitu.nombre=GameObject.Find("Mitu");
        mitu.ID=30;//no debe poderse escoger

        ciudades[30].nombre=GameObject.Find("Leticia");
        leticia.nombre=GameObject.Find("Leticia");
        leticia.ID=0;//no debe poderse escoger
        */
        llenarListas();

     
        
    }

    public List<int> Profundidad(int nodoI,int nodoF)
    {
            List<int> recorrido = new List<int>();
            List<int> cola = new List<int>();
            visited[nodoI] = true;

            recorrido.Add(nodoI);
            cola.Add(nodoI);
            while (cola.Count > 0 && cola[0]!=nodoF && encontrado==false)
            {
                int j = cola[0];
                cola.RemoveAt(0);
                for (int i = 0; i < 32; i++)
                {
                    if (mapa[j, i] == 1 && !visited[i])
                    {
                        cola.Add(i);
                        recorrido.AddRange(Profundidad(i,nodoF));
                        visited[i] = true;
                        if (encontrado)
                        {
                            i = 32;
                        }
                    }
                }

            }
            if (cola.Count > 0 && nodoF == cola[0])
            {
                encontrado = true;
            }
            return recorrido;

        }


    public void DFS(List<int> ruta)
    {
        
            rutaF=ruta;
            texto.GetComponent<Text>().text=""  ;
           
           int aux1=rutaF.Count;
           int aux=0;
            for(int i=0;i<cantNodos;i++)       
            {
             
                if(ciudades[i].ID==rutaF[aux]&&aux<aux1)
                {
                    aux++;
                    texto.GetComponent<Text>().text+=aux+") "+ciudades[i].city.ToString()+"\t";                    
                    i=0;
                    if (aux==aux1)
                    {
                        break;
                        
                    }
                }
            }
            btnRutacorta.SetActive(true);

            pintar (rutaF);
    }
    public void DD_Icambia(int val)
    {
        inicio=ciudades[val].ID;
    }
    public void DD_Fcambia(int val)
    {
        final=ciudades[val].ID;
    }

      public void RutaCorta()
    {
        
        int[,] tabla = new int[cantNodos, 3];
            /*Col   Ref
             * 0    visitado? (0=no;1=si)
             * 1    distancia entre nodos
             * 2    Nodo padre
             */
            for(n=0;n<cantNodos;n++)
            {
                tabla[n, 0] = 0;    //no visitado
                tabla[n, 1] = int.MaxValue;//Distancia
                tabla[n, 2] = 0;//nodo padre

            }
            tabla[inicio, 1] = 0;
            //MostrarTabla(tabla);

            for(dist=0;dist<cantNodos;dist++)//para recorrer todos los nodos
            {
                for(n=0;n<cantNodos;n++)
                {
                    if((tabla[n,0]==0)&&(tabla[n,1]==dist))//no ha sido visitado / menor distancia
                    {
                        tabla[n, 0] = 1;// se visito!
                        for(m=0;m<cantNodos;m++)//conexion con nodos
                        {
                            if(mapita.obtener(n,m)==1)//tiene conexion?
                            {
                                if(tabla[m,1]==int.MaxValue)
                                {
                                    tabla[m, 1] = dist + 1;//distancia "real"
                                    tabla[m, 2] = n;//nodo padre
                                }
                            }
                        }
                    }
                }
                cont++;
            }
            Debug.Log("hecho");
            //ruta final
            List<int> ruta = new List<int>();
            int nod = final;
            if(cont>=prof)
            {
                Console.Write("No hay rutas disponibles para ese destino");

            }
            else
            {
                while (nod != inicio)
                {
                    ruta.Add(nod);
                    nod = tabla[nod, 2];
                }

                ruta.Add(inicio);
                ruta.Reverse();
            }
            rutaC=ruta;
            textoF.GetComponent<Text>().text=""  ;
           
           int aux1=rutaC.Count;
           int aux=0;
            for(int i=0;i<cantNodos;i++)       
            {
             
                if(ciudades[i].ID==rutaC[aux]&&aux<aux1)
                {
                    aux++;
                    textoF.GetComponent<Text>().text+=aux+") "+ciudades[i].city.ToString()+"\n";                    
                    i=0;
                    if (aux==aux1)
                    {
                        break;
                        
                    }
                }
            }
            
            btnRutacorta.SetActive(false);
            pintar (rutaC);
    }
    public void llenarListas()
    {   
        
        for(int i=0;i<ciudades.Length;i++)
        {
            DDInicio.options.Add(new Dropdown.OptionData(){text=ciudades[i].city});
            DDfinal.options.Add(new Dropdown.OptionData(){text=ciudades[i].city});
            
        }
        for(int i=0;i<integrantes.Length;i++)
        {
            DDintegrantes.options.Add(new Dropdown.OptionData(){text=integrantes[i].ToString()});
        }
       

    }

    public void calcular()
    {
        cont=0;
        encontrado=false;
        visited = new bool[32];
       // btnRutacorta.SetActive(false);
        if(rutaF.Count!=0)
        {
            REsetpintar(rutaF);
        }
        if(rutaC.Count!=0)
        {
            REsetpintar(rutaC);
        }
        List <int> resp= new List<int>();
        resp =Profundidad(inicio,final);

        DFS(resp);
        

    }
    public void calcularRC()
    {

        if(rutaF.Count!=0)
        {
            REsetpintar(rutaF);
        }
        if(rutaC.Count!=0)
        {
            REsetpintar(rutaC);
        }

        RutaCorta();

    }

 public void REsetpintar(List<int> ruta)
    {
        int aux=ruta.Count;
        int aux2=1;

        Renderer Ninicio;
        Renderer Nfin;
        Renderer Nmedio;
        for (int i = 0; i < cantNodos; i++)
        {
            if(ciudades[i].ID==ruta[0])
            {
                Ninicio= ciudades[i].nombre.GetComponent<Renderer>();
                Ninicio.material=Mreset;
                break;
            }
            
        }
        for (int i = 0; i < cantNodos; i++)
        {
            if(ciudades[i].ID==ruta[aux-1])
            {
                Nfin= ciudades[i].nombre.GetComponent<Renderer>();
                Nfin.material=Mreset;
                break;
            }
            
        }   
         for(int i=0;i<cantNodos;i++)       
        {
            if(aux2>aux-2)
            {
                break;
            }
            else
            {
            if(ciudades[i].ID==ruta[aux2]&&aux2<=aux-2)
            {
                Nmedio=ciudades[i].nombre.GetComponent<Renderer>();
                Nmedio.material=Mreset;
                aux2++;
                i=0;
            }
            }

        }    
        

    }
   public void pintar(List<int> ruta)
    {
        int aux=ruta.Count;
        int aux2=1;
        Renderer Ninicio;
        Renderer Nfin;
        Renderer Nmedio;
        for (int i = 0; i < cantNodos; i++)
        {
            if(ciudades[i].ID==ruta[0])
            {
                Ninicio= ciudades[i].nombre.GetComponent<Renderer>();
                Ninicio.material=MInicio;
                break;
            }
            
        }
        for (int i = 0; i < cantNodos; i++)
        {
            if(ciudades[i].ID==ruta[aux-1])
            {
                Nfin= ciudades[i].nombre.GetComponent<Renderer>();
                Nfin.material=MFin;
                break;
            }
            
        }
          for(int i=0;i<cantNodos;i++)       
        {
            
            if(aux2>aux-2)
            {
                break;
            }
            else
            {
            if(ciudades[i].ID==ruta[aux2]&&aux2<=aux-2)
            {
                Nmedio=ciudades[i].nombre.GetComponent<Renderer>();
                Nmedio.material=MMedio;
                aux2++;
                i=0;
            }
            }

        }
      


    }
    public void Exit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
       
        
        
    }
}
