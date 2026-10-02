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
    public double GetAtomNumber()
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
        return null;
    }
    
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
        
    }
    class DecaySimulation
    {
        static void Main()
        {
            LinkedList<double> unDecayed = new LinkedList<double>();
            int DecayTrials = 1000;
            Random rng = new Random();
            double NonDecayed = RunMonteCarloRound(DecayTrials, rng);

            while (unDecayed.head != null)
            {
                unDecayed=RunMonteCarloRound(unDecayed);
                 sweepList(unDecayed);
                wait 1;
            }


        }

        // Recursive Monte Carlo function
        static LinkedList<double> RunMonteCarloRound(LinkedList<double> PassedDecayList)
        {
            LinkedNode<double> CurrentNode = PassedDecayList.head;
            if (CurrentNode != null)
            {
                if (Testdecay())
                {
                    BroadcastDecay(CurrentNode);
                    PassedDecayList.Remove(CurrentNode);
                    PassedDecayList=RunMonteCarloRound(PassedDecayList);
                }
            }
            return PassedDecayList;
        }
    }
}