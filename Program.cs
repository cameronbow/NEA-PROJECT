
using Cairo;
using Gdk;
using Gtk;
using Pango;
using System;
using System.Reflection.Emit;
using static SelectionBox;
using static SelectionBox.Coordinate;
public class Background : Gtk.Window
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






public class SelectionBox : Gtk.Window
{

    Gtk.Scale speedslider = new Gtk.Scale(Orientation.Horizontal, 1, 1000, 1);


    Gtk.Scale atomslider = new Gtk.Scale(Orientation.Horizontal, 0, 100000, 1);

    // Add these fields to the SelectionBox class to fix CS0117
    private RadioButton option1;
    private RadioButton option2;
    private RadioButton option3;

    public SelectionBox() : base("Selection Box")
    {



        SetDefaultSize(400, 300);


        //Creates a new colour
        Gdk.Color Gray = new Gdk.Color();

        //assigns hex code to colour
        Gdk.Color.Parse("#808080", ref Gray);

        //adds colour to selction box
        //ModifyBg(StateType.Normal, Gray);

        //creates the speed slider lable
        Gtk.Label SpeedsliderLabel = new Gtk.Label("Speed:");

        // Set a fixed size for the label
        SpeedsliderLabel.SetSizeRequest(100, -1);

        //creates the number of atoms slider lable
        Gtk.Label AtomsliderLabel = new Gtk.Label("# of Atoms:");

        // Set a fixed size for the label
        AtomsliderLabel.SetSizeRequest(100, -1);

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

        // creates first option
        option1 = new RadioButton("Uranium235");

        // creates second option linking with the first
        option2 = new RadioButton(option1, "Carbon14");

        // creates third option linking with the first
        option3 = new RadioButton(option1, "Radon222");

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

    public string Radiobuttonseleted()
    {

        // Check which radio button is active
        if (option1.Active)
        {
            Console.WriteLine("Option 1 selected");
            return option1.Label;
        }
        else if (option2.Active)
        {
            Console.WriteLine("Option 2 selected");
            return option2.Label;
        }
        else if (option3.Active)
        {
            Console.WriteLine("Option 3 selected");
            return option3.Label;
        }
        else
            Console.WriteLine("No option selected");
        return null;
    }

    //creates a function to retrive the number of atoms chosen by the user
    public double GetAtomNumber()
    {
        double atoms;
        atoms = atomslider.Value;
        Console.WriteLine(atoms);
        return atoms;
    }

    public int GetDeltaTime()
    {
        int deltaTime;
        deltaTime = (int)speedslider.Value;
        Console.WriteLine(deltaTime);
        return deltaTime;
    }
    //creates a function that was linked to earlier in the code for when the start simulation button is clicked
    private void OnButtonClicked(object sender, EventArgs e)
    {
        //runs the GetAtomNumber function
        int numberOfAtoms = Convert.ToInt32(GetAtomNumber());
        string optionselcted = Radiobuttonseleted();
        //starts the simulation passing it the amount of atoms and list of atom attributes
        StartSimulation(numberOfAtoms, GetAtomLists(optionselcted));


    }

    public void StartSimulation(int passedAtoms, LinkedList<double> passedAtomtype)
    {
        //takes the decay constant out of the first position in the list
        double decayConstant = passedAtomtype.ListIndexer(0);

        //takes the decay type out of the second position and unpacks it into its name
        string decaytype = DepackDecayType((int)passedAtomtype.ListIndexer(1));

        //takes the atomic number out of the third position
        int atomicNumber = (int)passedAtomtype.ListIndexer(2);

        //runs the simulation passing it the number of atoms and the decay constant
        DecaySimulation.Decaysimulation(passedAtoms, decayConstant, (Math.Log(2) / decayConstant) * GetDeltaTime() / 100.0);
    }

    //this method will create all the atom type linked lists, pick one and return it
    public LinkedList<double> GetAtomLists(string passedAtomname)
    {
        //creates the Uranium235 linked list
        LinkedList<double> Uranium235 = new LinkedList<double>();

        //decay constant in years, worked out from the half life of 703.8 million years
        Uranium235.Insert(Math.Log(2) / 703800000.0);

        //decay type unpacked by a seperate method, 1 is alpha
        Uranium235.Insert(1);

        //atomic number, for 1 atom special cases
        Uranium235.Insert(92);


        //creates the Carbon14 linked list
        LinkedList<double> Carbon14 = new LinkedList<double>();

        //decay constant in years, half life is 5730 years
        // Carbon14.Insert(Math.Log(2) / 5730.0);
        Carbon14.Insert(Math.Log(2) / 5730.0);
        //decay type, 2 is beta
        Carbon14.Insert(2);

        //atomic number
        Carbon14.Insert(6);


        //creates the Radon222 linked list
        LinkedList<double> Radon222 = new LinkedList<double>();

        //decay constant in years, half life is 3.82 days so it is divided by 365.25 to get years
        Radon222.Insert(Math.Log(2) / (3.82 / 365.25));

        //decay type, 1 is alpha
        Radon222.Insert(1);

        //atomic number
        Radon222.Insert(86);


        //switching based on the desired atom name
        switch (passedAtomname)
        {
            case "Uranium235":
                {
                    return Uranium235;
                }
            case "Carbon14":
                {
                    return Carbon14;
                }
            case "Radon222":
                {
                    return Radon222;
                }
        }
        //null case
        return null;
    }

    //the linkedlist is a list of intigers so this function depacks the decaytype integer into its actual string name
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

    public class Simulationbox : Gtk.Window
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

            int panelHeight = scHeight;

            //asign the height of the panel to be the whole length of the users screen
            SetDefaultSize((scWidth - 475), panelHeight);

            //moving panel to side of screen
            Move(0, 0);

            //creates the drawing area for the simulation
            Gtk.DrawingArea drawingArea = new Gtk.DrawingArea();

            //adds the drawing area to the simulation window
            Add(drawingArea);

            //connects the drawing area to the simulation
            DecaySimulation.SetDrawingArea(drawingArea);

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
        public void RemoveCorrd(LinkedNode<Coordinate> NodeToRemove)
        {
            //gets the coordinates and attributes of the atom
            Coordinate CurrentCoord = NodeToRemove.data;

            //changes the atom to decayed
            CurrentCoord.decayed = true;

            //updates the data stored in the node
            NodeToRemove.data = CurrentCoord;
        }

        public int Length(LinkedList<Coordinate> MeasuringList)
        {
            int length = 0;
            LinkedNode<Coordinate> current = MeasuringList.head;
            while (current != null)
            {
                length++;
                current = current.next;
            }
            return length;
        }
        public T? ListIndexer(int indexNum)
        {
            if (indexNum < 0)
                throw new ArgumentOutOfRangeException(nameof(indexNum));
            LinkedNode<T> current = this.head;
            for (int i = 0; i < indexNum; i++)
            {
                if (current == null)
                    throw new ArgumentOutOfRangeException(nameof(indexNum));
                current = current.next;
            }
            if (current == null)
                throw new ArgumentOutOfRangeException(nameof(indexNum));
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
                FrontItem = Current;
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
            if (FrontItem == null)
                throw new InvalidOperationException("Queue is empty");
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

        public int id;
        public bool decayed;
        public Coordinate(int xpass, int ypass, int idpass)
        {
            this.x = xpass;
            this.y = ypass;
            this.decayed = false;
            this.id = idpass;
        }
        public class DecayParticle
        {
            //stores the coordinates of the particle
            public float x;
            public float y;

            //stores how much the particle moves each update
            public float speedX;
            public float speedY;

            //creates a new particle
            public DecayParticle(float x, float y)
            {
                //sets the starting coordinates
                this.x = x;
                this.y = y;

                //sets the direction the particle travels
                speedX = 2;
                speedY = -2;
            }
        }

    }
    static class DecaySimulation
    {
        //stores the GTK drawing area and the atoms displayed on it
        static Gtk.DrawingArea drawingArea;
        static LinkedList<Coordinate> drawingAtoms;
        static bool drawingTimerRunning = false;
        static Random generator = new Random();

        //creates a linked list to store all the particles released by decay
        static LinkedList<DecayParticle> DecayedParticels = new LinkedList<DecayParticle>();
        static bool simulationRunning = false;
        public static void Decaysimulation(int AtomNumber, double decayConstant, double deltaTime)
        {



            //creates a new linked list of coordinates to hold the undecayed atoms
            LinkedList<Coordinate> unDecayed = new LinkedList<Coordinate>();

            //fills the linked list with the number of atoms
            unDecayed = FillList(unDecayed, AtomNumber);

            //creates a list to store every atom for drawing
            LinkedList<Coordinate> allAtoms = new LinkedList<Coordinate>();

            //copies all the atoms into the drawing list
            LinkedNode<Coordinate> CurrentNode = unDecayed.head;

            while (CurrentNode != null)
            {
                allAtoms.Insert(CurrentNode.data);
                CurrentNode = CurrentNode.next;
            }

            //runs the monte carlo simulation until all atoms have decayed
            if (simulationRunning || AtomNumber <= 0 || deltaTime <= 0)
                return;
            simulationRunning = true;

            //passes the full atom list to the drawing area
            drawingAtoms = allAtoms;

            //starts the drawing timer
            StartDrawing();

            GLib.Timeout.Add(1000, delegate
            {
                Console.WriteLine("Number of undecayed atoms: " + AtomNumber);
                unDecayed.DisplayLL(unDecayed);
                //runs the monte carlo simulation round and returns the new list of undecayed atoms
                unDecayed = RunMonteCarloRound(unDecayed, unDecayed.head, decayConstant, deltaTime);

                //updates the drawing list with decayed atoms
                UpdateDrawingList(allAtoms, unDecayed);

                //sweeps the list to remove any decayed atoms
                unDecayed = SweepList(unDecayed);
                AtomNumber = unDecayed.Length(unDecayed);

                unDecayed.DisplayLL(unDecayed);
                if (AtomNumber == 0)
                    simulationRunning = false;
                return AtomNumber > 0;
            }
            );
        }

        // Recursive Monte Carlo function
        static LinkedList<Coordinate> RunMonteCarloRound(LinkedList<Coordinate> PassedDecayList, LinkedNode<Coordinate> currentNode, double decayConstant, double deltaTime)
        {
            //stops when the end of the list is reached
            if (currentNode != null)
            {
                //tests if this atom decays using the decay constant and the time step
                if (MonteCarloTestDecay(decayConstant, deltaTime))
                {
                    //tells the other atoms this one has decayed
                    BroadcastDecay(currentNode);

                    //marks the atom as removed by setting its coordinates to 0
                    PassedDecayList.RemoveCorrd(currentNode);
                }

                //moves to the next atom by calling the function again
                PassedDecayList = RunMonteCarloRound(PassedDecayList, currentNode.next, decayConstant, deltaTime);
            }
            return PassedDecayList;
        }
        static void UpdateDrawingList(
    LinkedList<Coordinate> allAtoms,
    LinkedList<Coordinate> unDecayed)
        {
            //starts at the head of the undecayed list
            LinkedNode<Coordinate> CurrentNode = unDecayed.head;

            //loops through every atom in the undecayed list
            while (CurrentNode != null)
            {
                //checks if the atom has decayed
                if (CurrentNode.data.decayed == true)
                {
                    //starts at the head of the drawing list
                    LinkedNode<Coordinate> DrawingNode = allAtoms.head;

                    //loops through the drawing list
                    while (DrawingNode != null)
                    {
                        //checks if the atom IDs match
                        if (DrawingNode.data.id == CurrentNode.data.id)
                        {
                            //gets the coordinates and attributes of the atom
                            Coordinate CurrentCoord = DrawingNode.data;

                            //marks the atom as decayed
                            CurrentCoord.decayed = true;

                            //updates the atom in the drawing list
                            DrawingNode.data = CurrentCoord;
                        }

                        //moves to the next atom in the drawing list
                        DrawingNode = DrawingNode.next;
                    }
                }

                //moves to the next atom in the undecayed list
                CurrentNode = CurrentNode.next;
            }
        }
        static void BroadcastDecay(LinkedNode<Coordinate> CurrentNode)
        {
            //gets the coordinates of the atom that has decayed
            Coordinate CurrentCoord = CurrentNode.data;

            //creates a new particle at the position of the decayed atom
            DecayParticle NewParticle = new DecayParticle(
                CurrentCoord.x * 15,
                CurrentCoord.y * 15
            );

            //adds the new particle to the linked list
            DecayedParticels.Insert(NewParticle);
        }
        public static void DrawDecayParticles(Cairo.Context cr)
        {
            //creates a node starting at the head of the Decayed particles list
            LinkedNode<DecayParticle> CurrentNode = DecayedParticels.head;

            //sets the colour of decayed particles to yellow
            cr.SetSourceRGB(1, 1, 0);

            //loops through every decayed particle
            while (CurrentNode != null)
            {
                //gets the current particle
                DecayParticle CurrentParticle = CurrentNode.data;

                //changes the coordinates to move the particle
                CurrentParticle.x = CurrentParticle.x + CurrentParticle.speedX;
                CurrentParticle.y = CurrentParticle.y + CurrentParticle.speedY;

                //creates a rectangle at the particles new coordinates
                cr.Rectangle(CurrentParticle.x, CurrentParticle.y, 4, 4);

                //draws the decayed particle
                cr.Fill();

                //moves to the next particle in the list
                CurrentNode = CurrentNode.next;
            }
        }

        // tests if an atom decays using monte carlo
        static bool MonteCarloTestDecay(double decayConstant, double ChangeInTime)
        {
            // calculates decay chance
            double decayProbability = 1 - Math.Exp(-decayConstant * ChangeInTime);

            // generates a random numberq
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
            if (AtomNumber <= 0)
                return FillingList;
            // sets width to 0 initally
            int width = 0;

            // width is set to the squre root of the atom total
            width = (int)Math.Ceiling(Math.Sqrt(AtomNumber));

            // will loop until all atoms are added to list

            for (int i = 0; i < AtomNumber; i++)
            {
                Coordinate data = new Coordinate((i % width) + 1, (i / width) + 1, i);
                FillingList.Insert(data);
            }

            // returns list when full
            return FillingList;


        }
        static LinkedList<Coordinate> SweepList(LinkedList<Coordinate> ListToSweep)
        {
            //creates a queue to temporarily store undecayed atoms
            LinkedQueue<Coordinate> SweepingQueue = new LinkedQueue<Coordinate>();

            //starts at the head of the list
            LinkedNode<Coordinate> CurrentNode = ListToSweep.head;

            //creates a new linked list to return
            LinkedList<Coordinate> returnList = new LinkedList<Coordinate>();

            //gets the length of the list
            int lengthOfList = ListToSweep.Length(ListToSweep);
            int count = 0;

            //loops through every atom in the list
            while (count < lengthOfList)
            {
                //checks if the atom has not decayed
                if (IsEmpty(CurrentNode) == false)
                {
                    //adds the undecayed atom to the queue
                    SweepingQueue.Enqueue(CurrentNode.data);
                }

                //moves to the next atom
                CurrentNode = CurrentNode.next;
                count++;


                //loops until the queue is empty
                while (SweepingQueue.count() > 0)
                {
                    //removes the first item from the queue and adds it to the new list
                    returnList.Insert(SweepingQueue.Dequeue());
                }
            }
            return returnList;
        }
public static bool IsEmpty(LinkedNode<Coordinate> testingNode)
{
    //checks if the atom has decayed
    if (testingNode.data.decayed == true)
    {
        return true;
    }

    return false;
}
public static void SetDrawingArea(Gtk.DrawingArea passedDrawingArea)
{
    //stores the drawing area from the simulation window
    drawingArea = passedDrawingArea;

    //links the drawing function to the drawing area
    drawingArea.Drawn += OnDrawing;
}

static void OnDrawing(object sender, Gtk.DrawnArgs args)
{
    //draws the atoms when GTK updates the drawing area
    if (drawingAtoms != null)
    {
        DrawPartic(args.Cr, drawingAtoms);
    }
}

public static void StartDrawing()
{
    //checks if the drawing timer is already running
    if (drawingTimerRunning == true)
        return;

    drawingTimerRunning = true;

    //updates the drawing area approximately 60 times per second
    GLib.Timeout.Add(16, delegate
    {
        //requests GTK to draw the next frame
        if (drawingArea != null)
            drawingArea.QueueDraw();

        //keeps the drawing timer running
        return true;
    });
}

public static void DrawPartic(Cairo.Context cr, LinkedList<Coordinate> unDecayed)
{
    //sets the background colour to black
    cr.SetSourceRGB(0, 0, 0);
    cr.Paint();

    //creates a node starting at the head of the list
    LinkedNode<Coordinate> CurrentNode = unDecayed.head;

    //loops through each atom in the list
    while (CurrentNode != null)
    {
        //gets the coordinates of the current atom
        Coordinate CurrentCoord = CurrentNode.data;

        //checks if the atom has decayed
        if (CurrentCoord.decayed == true)
        {
            //sets the colour to red
            cr.SetSourceRGB(1, 0, 0);
        }
        else
        {
            //sets the colour to blue
            cr.SetSourceRGB(0, 0, 1);
        }

        //creates a rectangle at the atoms coordinates
        cr.Rectangle(CurrentCoord.x * 15, CurrentCoord.y * 15, 8, 8);

        //draws the atom
        cr.Fill();

        //moves to the next atom in the list
        CurrentNode = CurrentNode.next;
    }

    //draws all the particles released by decay
    DrawDecayParticles(cr);
}
    }

}

