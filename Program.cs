using Gtk;

using SDL3;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;
using static SelectionBox;


public class Background : Window
{
    public Background() : base("Background")
    {


        SetDefaultSize(400, 300);
        Maximize();


        //Creates a new colour
        Gdk.Color White = new Gdk.Color();

        //assigns hex code to colour
        Gdk.Color.Parse("#FFFFFF", ref White);

        //adds colour to Background 
        ModifyBg(StateType.Normal, White);

        ShowAll();
    }
}






public class SelectionBox : Window
{
    //creates the actual slider going from 0 to 100 in incriments of 1
    Gtk.Scale speedslider = new Gtk.Scale(Orientation.Horizontal, 0, 100, 1);

    //creates the actual slider going from 0 to 100 in incriments of 1
    Gtk.Scale atomslider = new Gtk.Scale(Orientation.Horizontal, 0, 100, 1);

    public SelectionBox() : base("Selection Box")
    {



        SetDefaultSize(400, 300);


        //Creates a new colour
        Gdk.Color Gray = new Gdk.Color();

        //assigns hex code to colour
        Gdk.Color.Parse("#808080", ref Gray);

        //adds colour to selction box
        ModifyBg(StateType.Normal, Gray);

        //creates the speed slider lable
        Label SpeedsliderLabel = new Label("Speed:");

        //creates the number of atoms slider lable
        Label AtomsliderLabel = new Label("# of Atoms:");

        //creates "vertical box" but think about it as a virtual box that each smaller component can be placed in
        VBox box = new VBox(false, 10);

        //sets the size of each virtical box
        box.BorderWidth = 20;

        //speed slider inital value
        speedslider.Value = 50;

        //atom slider inital value
        atomslider.Value = 50;

        //puts the the lable in the first virtical box
        box.PackStart(SpeedsliderLabel, false, false, 0);

        //puts the slider underneath in the second vertical box
        box.PackStart(speedslider, false, false, 0);

        //puts the the lable in the first virtical box
        box.PackStart(AtomsliderLabel, false, false, 0);

        //puts the slider underneath in the second vertical box
        box.PackStart(atomslider, false, false, 0);

        //creates first option
        RadioButton option1 = new RadioButton("Option 1");

        //creates second option linking with the first
        RadioButton option2 = new RadioButton(option1, "Option 2");

        //creates third option linking with the first
        RadioButton option3 = new RadioButton(option1, "Option 3");

        //creates start simulation button
        Button SimualtionStart = new Button("SIMULATION START");

        //links function to when button is clicked
        SimualtionStart.Clicked += OnButtonClicked;


        //has the first option selcted by defult
        option1.Active = true;

        //puts all buttons under lable and slider and then in order
        box.PackStart(option1, false, false, 0);
        box.PackStart(option2, false, false, 0);
        box.PackStart(option3, false, false, 0);

        //adds Simulationstart button under radio buttons
        box.PackStart(SimualtionStart, false, false, 0);

        //adds the virtical boxs to the window
        Add(box);



        // Get the screen size and asign values to variables
        Gdk.Screen screen = Gdk.Screen.Default;
        int scWidth = screen.Width;
        int scHeight = screen.Height;

        //asign the thickness to be 450 pixels
        int panelWidth = 450;


        //asign the height of the panel to be the whole length of the users screen
        SetDefaultSize(panelWidth, scHeight);
        SetSizeRequest(panelWidth, scHeight);

        //moving panel to side of screen
        Move(scWidth - panelWidth, 0);

        //shows everything
        ShowAll();

    }

    //creates a function to retrive the number of atoms chosen by the user
    public  double GetAtomNumber()
    {
        double atoms;
        atoms = atomslider.Value;
        Console.WriteLine(atoms);
        return atoms;
    }

    //creates a function that was linked to earlier in the code for when the start simulation button is clicked
    private void OnButtonClicked(object sender, EventArgs e)
    {
        //runs the GetAtomNumber function
        int numberOfAtoms = Convert.ToInt32(GetAtomNumber());
        //starts the simulation passing it the amout of atoms and lsit of atom attributes
        StartSimulation(numberOfAtoms, GetAtomLists("Uranium235"));
    }
    public void StartSimulation(int passedAtoms, LinkedList<int> passedAtomtype)
    {
        int decayConstant = passedAtomtype.ListIndexer(0);
        string decaytype = DepackDecayType(passedAtomtype.ListIndexer(1));
        int atomicNumber = passedAtomtype.ListIndexer(2);
        DecaySimulation.Decaysimulation();


    }
    
    //this method will create all the atom type linked list, pick one and return in
    public LinkedList<int> GetAtomLists(string passedAtomname)
    {
        LinkedList<int> Uranium235 = new LinkedList<int>();

        // Decay constant in years
        Uranium235.Insert(1);

        //Decay type unpacked by a seperate method
        Uranium235.Insert(2);

        //atomic number, for 1 atom specaial caseses
        Uranium235.Insert(3);

        LinkedList<int> f = new LinkedList<int>();
        f.Insert(1);
        f.Insert(2);
        f.Insert(3);
        LinkedList<int>  g = new LinkedList<int>();
        g.Insert(1);
        g.Insert(2);
        g.Insert(3);
        
        //switching basesd on the desisered atom name
        switch (passedAtomname)
        {
            case "Uranium235":
                {
                    return Uranium235;
                }
            case "f":
                {
                    return Uranium235;
                }
            case "g":
                {
                    return Uranium235;
                }

        }
        //null case
        return null;    }
    
    //the linkedlist is a list of intigers so thisfunction depacks the decaytype integer into its actual string name
    public string DepackDecayType(int PassedDecayValue)
    {
        switch (PassedDecayValue)
        {
            case 1:
                {
                    return "Alpha";
                }

            case 2:
                {
                    return "Beta";
                }

            case 3:
                {
                    return "Gamma";
                }

            //null case
            default: return null; 
        }
    }

    public class Simulationbox : Window
{
    public Simulationbox() : base("Simulation")
    {

        //sets the defult size
        SetDefaultSize(400, 300);



        //Creates a new colour
        Gdk.Color Black = new Gdk.Color();

        //assigns hex code to colour
        Gdk.Color.Parse("#000000", ref Black);

        //adds colour to Simulation window
        ModifyBg(StateType.Normal, Black);

        // Get the screen size and asign values to variables
        Gdk.Screen screen = Gdk.Screen.Default;
        int scWidth = screen.Width;
        int scHeight = screen.Height;

        int panelHeight = 950;

        //asign the height of the panel to be the whole length of the users screen
        SetDefaultSize((scWidth - 475), panelHeight);
        SetSizeRequest((scWidth - 475), panelHeight);

        //moving panel to side of screen
        Move(0, screen.Height - 50);

        ShowAll();
    }

}

    partial class Program
    {
    static void Main(string[] args)
    {

        Application.Init();

        // Create the background window
        Background background = new Background();

        // Create the selection window
        SelectionBox selectionBox = new SelectionBox();

        // Create the simulation window
        Simulationbox simulation = new Simulationbox();

        // Show all windows
        background.Show();
        selectionBox.Show();
        simulation.Show();

        Application.Run();

    }
    }

//below i am creating my own linked lists using genric type to allow for creating diffretn data type lists

//creates the node class
    public class LinkedNode<T>
    {
      //the data being stored at that node
      public T data;
    
     //the pointer to the next node
     public LinkedNode<T> next;

      //end node
       public LinkedNode(T data)
      {
          this.data = data;
          this.next = null;
      }
      //header and main body
      public LinkedNode(T data, LinkedNode<T> next)
     {
         this.data = data;
         this.next = next;
        }
    }

    //creating a class to use the nodes
    public class LinkedList<T>
    {
        //creating a node head
        public LinkedNode<T> head;

        //starting with it null
        public LinkedList()
        {
            this.head = null;
        }
        //insert into the linked list function
        public void Insert(T data)
        {
            //function creates a new new node with the data being passed
            LinkedNode<T> newNode = new LinkedNode<T>(data);

            //If the list has a head just add the item at the end
            if (head != null)
            {
                LinkedNode<T> current = this.head;
                while (current.next != null)
                {
                    current = current.next;
                }
                current.next = newNode;
            }
            //if this is the first item make it the head
            else
            {
                this.head = newNode;
            }
        }
        public void RemoveCorr(LinkedNode<Coordinate> NodeToRemove)
        {
            NodeToRemove.data.x = 0;
            NodeToRemove.data.y = 0;
        }
        public T? ListIndexer( int indexNum)
        {
            LinkedNode<T> current = this.head;
            if (current == null)
            {
                Console.WriteLine("this list is empty");
                return default(T);
            }

            for (int i = 0; i < indexNum; i++)
            {
                    current = current.next;
                }
                return current.data;
        }
        public void DisplayLL(LinkedList<Coordinate> DisplayList)
        {
            LinkedNode<Coordinate> CurrentNode = DisplayList.head;
            while (CurrentNode != null)
            {
                Console.WriteLine(CurrentNode.data.x + " " + CurrentNode.data.y);

                CurrentNode = CurrentNode.next;
                }
        }
    }
    public class LinkedQueue<T>
    {
        // First element
        private LinkedNode<T> FrontItem;

        // Last element
        private LinkedNode<T> LastItem;

        // Number of elements
        private int Count;

        //creates a clear queue
        public LinkedQueue()
        {
            FrontItem = LastItem = null;
            Count = 0;
        }
        // Add item to the end of the queue
        public void Enqueue(T item)
        {
            LinkedNode<T> Current = new LinkedNode<T>(item);


            // if Queue is empty make this item first and last
            if (LastItem == null) 
            {
                FrontItem =  Current;
                LastItem = Current;
            }
            else
            {
                //makes the pointer of the previous last item, point to this item
                LastItem.next = Current;

                //Make the current item the new last item
                LastItem = Current;
            }

            //increase the count in the queue
            Count++;
        }
        // Remove element from the queue
        public T Dequeue()
        {
            //takes the front value
            T value = FrontItem.data;

            //makes the next item the new front item
            FrontItem = FrontItem.next;

            //if this process empties thelist
            if (FrontItem == null) 
                LastItem = null;

            //remove the front item from the count
            Count--;

            //return the dequeed item
            return value;
        }

        public int count()
        {
            return Count;
        }


    }
    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
     

    }
    class DecaySimulation
    {
        public static void Decaysimulation()
        {
            //initates new slecion box to get the slider value
            SelectionBox selectionBox = new SelectionBox();

            //gets slider value and assigns to atom number
            int AtomNumber = Convert.ToInt32(selectionBox.GetAtomNumber());

            //creates a new linked list of doubles to hold the undecayed atoms
            LinkedList<Coordinate> unDecayed = new LinkedList<Coordinate>();

            //fills the linked list with the number of atoms
            unDecayed = FillList(unDecayed, AtomNumber);

            //creates a new random number generator
            Random rng = new Random();

            //runs the monte carlo simulation until all atoms have decayed
            while (unDecayed.head != null)
            {

                //runs the monte carlo simulation round and returns the new list of undecayed atoms
                unDecayed = RunMonteCarloRound(unDecayed, unDecayed.head);

                //sweeps the list to remove any decayed atoms
                unDecayed=SweepList(unDecayed);

                //sleeps
                Thread.Sleep(1000);
            }


        }

        // Recursive Monte Carlo function
        static LinkedList<Coordinate> RunMonteCarloRound(LinkedList<Coordinate> PassedDecayList, LinkedNode<Coordinate> currentNode)
        {
            PassedDecayList.DisplayLL(PassedDecayList);
            if (currentNode != null)
            {
                if (MonteCarloTestDecay())
                {
                    BroadcastDecay(currentNode);
                    PassedDecayList.RemoveCorr(currentNode);
                }
                currentNode = currentNode.next;
                PassedDecayList = RunMonteCarloRound(PassedDecayList, currentNode);
                
            }
            return PassedDecayList;
        }

        static void BroadcastDecay(LinkedNode<Coordinate> CurrentNode)
        {

        }


        // tests if an atom decays using monte carlo
        static bool MonteCarloTestDecay(double decayConstant, double ChangeInTime)
        {
            // creates a random number generator
             Random generator = new Random();
            // calculates decay chance
            double decayProbability = 1 - Math.Exp(-decayConstant * ChangeInTime);
            // generates a random number
            double randomValue = generator.NextDouble();
            // checks if the atom decays
            if (randomValue < decayProbability)
            {
                // atom decays
                return true;
            }
            else
            {
                // atom does not decay
                return false;
            }
        }
        public static LinkedList<Coordinate> FillList(LinkedList<Coordinate> FillingList, int AtomNumber)
        {
           // sets width to 0 initally
           int width = 0;



            // width is set to the squre root of the atom total
            width = (int)Math.Floor(Math.Sqrt(AtomNumber));

            // will loop until all atoms are added to list
                for (int x = 1; x <= (AtomNumber/width); x++)
                {

            // loops until width is reached
                    for (int y = 1; y < width; y++)
                    {

                    // delcares data as the cooridnates type Coordinate, a specially created data type
                    Coordinate data = new(x,y);

                    //inserts coordinate into list
                    FillingList.Insert(data);
                    }
                }
            
            // returns list when full
            return FillingList;


        }
         static LinkedList<Coordinate> SweepList(LinkedList<Coordinate> ListToSweep)
        {
         LinkedQueue<Coordinate> SweepingQueue = new LinkedQueue<Coordinate>();
            LinkedNode<Coordinate> CurrentNode = ListToSweep.head;
            LinkedList < Coordinate > returnList = new LinkedList<Coordinate>();
           while (CurrentNode != null)
           {
                if (IsEmpty(CurrentNode) == false)
                {
                    SweepingQueue.Enqueue(CurrentNode.data);
                }
                    CurrentNode = CurrentNode.next;
            }
           for (int i = 0; i > SweepingQueue.count(); i++)
            {
                returnList.Insert(SweepingQueue.Dequeue());
            }
                return returnList;
        }
        public static bool IsEmpty(LinkedNode<Coordinate> testingNode)
        {
            if ((testingNode.data.x == 0) && (testingNode.data.y == 0))
            {
                return true;
            }
            return false;
        }
    }
}