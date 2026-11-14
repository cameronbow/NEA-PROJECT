
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
    Gtk.Scale atomslider = new Gtk.Scale(Orientation.Horizontal, 0, 10000, 1);

    //stores the isotope selection box
    private VBox IsotopeBox;

    //stores the currently selected isotope
    private string selectedIsotope = "Uranium235";

    public SelectionBox() : base("Selection Box")
    {
        SetDefaultSize(400, 300);

        //Creates a new colour
        Gdk.Color Gray = new Gdk.Color();

        //assigns hex code to colour
        Gdk.Color.Parse("#808080", ref Gray);

        //adds colour to selection box
        //ModifyBg(StateType.Normal, Gray);

        //creates the speed slider label
        Gtk.Label SpeedsliderLabel = new Gtk.Label("Speed (time):");

        // Set a fixed size for the label
        SpeedsliderLabel.SetSizeRequest(100, -1);
        SpeedsliderLabel.Xalign = 0;

        //creates the number of atoms slider label
        Gtk.Label AtomsliderLabel = new Gtk.Label("# of Atoms (atomnum):");

        // Set a fixed size for the label
        AtomsliderLabel.SetSizeRequest(100, -1);
        AtomsliderLabel.Xalign = 0;

        //creates vertical box for components
        VBox box = new VBox(false, 15);

        //sets the size of the vertical box
        box.BorderWidth = 20;

        //speed slider initial value
        speedslider.Value = 50;

        //atom slider initial value
        atomslider.Value = 50;

        //creates a frame for the simulation controls
        Frame ControlFrame = new Frame("Simulation Settings");

        //creates a vertical box for the sliders
        VBox ControlBox = new VBox(false, 10);

        //creates a drop-down explanation for time
        Gtk.Expander TimedropDown = new Gtk.Expander("What does time do?");

        //creates the time explanation text
        Gtk.Label TimeDropDownText = new Gtk.Label("in years, how quickly the simulation progresses. ");

        //allows the explanation text to wrap
        TimeDropDownText.LineWrap = true;

        //adds the explanation text to the drop-down
        TimedropDown.Add(TimeDropDownText);

        //creates a drop-down explanation for atomnum
        Gtk.Expander AtomsDropDOwn = new Gtk.Expander("What does atomnum mean?");

        //creates the atomnum explanation text
        Gtk.Label AtomDropDOwnText = new Gtk.Label("Controls the number of radioactive atoms ");

        //allows the explanation text to wrap
        AtomDropDOwnText.LineWrap = true;

        //adds the explanation text to the drop-down
        AtomsDropDOwn.Add(AtomDropDOwnText);

        //puts the speed slider label in the box
        ControlBox.PackStart(SpeedsliderLabel, false, false, 0);

        //puts the speed slider underneath the label
        ControlBox.PackStart(speedslider, false, false, 0);

        //adds the time explanation drop-down
        ControlBox.PackStart(TimedropDown, false, false, 0);

        //puts the atom slider label in the box
        ControlBox.PackStart(AtomsliderLabel, false, false, 0);

        //puts the atom slider underneath the label
        ControlBox.PackStart(atomslider, false, false, 0);

        //adds the atomnum explanation drop-down
        ControlBox.PackStart(AtomsDropDOwn, false, false, 0);

        //adds the controls to the frame
        ControlFrame.Add(ControlBox);

        //creates a frame for the isotope selection
        Frame IsotopeFrame = new Frame("Radioactive Isotope");

        //creates a vertical box for isotope selection
        VBox IsotopeSelectionBox = new VBox(false, 10);

        //creates a horizontal box for decay type buttons
        HBox DecayTypeBox = new HBox(false, 5);

        //creates the alpha button
        Button AlphaButton = new Button("Alpha");

        //creates the beta button
        Button BetaButton = new Button("Beta");

        //creates the gamma button
        Button GammaButton = new Button("Gamma");

        //puts the decay buttons into the horizontal box
        DecayTypeBox.PackStart(AlphaButton, true, true, 0);
        DecayTypeBox.PackStart(BetaButton, true, true, 0);
        DecayTypeBox.PackStart(GammaButton, true, true, 0);

        //puts the decay buttons at the top of the isotope selection
        IsotopeSelectionBox.PackStart(DecayTypeBox, false, false, 0);

        //creates a vertical box for the isotope radio buttons
        IsotopeBox = new VBox(false, 10);

        //puts the isotope radio buttons underneath the decay buttons
        IsotopeSelectionBox.PackStart(IsotopeBox, false, false, 0);

        //links the alpha button to its function
        AlphaButton.Clicked += AlphaButtonClicked;

        //links the beta button to its function
        BetaButton.Clicked += BetaButtonClicked;

        //links the gamma button to its function
        GammaButton.Clicked += GammaButtonClicked;

        //shows alpha isotopes by default
        ShowIsotopes("Alpha");

        //adds the isotope selection to the frame
        IsotopeFrame.Add(IsotopeSelectionBox);

        //creates start simulation button
        Button SimualtionStart = new Button("SIMULATION START");

        //links function to when button is clicked
        SimualtionStart.Clicked += OnButtonClicked;

        //sets the size of the start button
        SimualtionStart.SetSizeRequest(-1, 45);

        //puts the controls frame in the main box
        box.PackStart(ControlFrame, false, false, 0);

        //puts the isotope frame underneath the controls
        box.PackStart(IsotopeFrame, false, false, 0);

        //adds SimulationStart button underneath radio buttons
        box.PackStart(SimualtionStart, false, false, 0);

        //adds the vertical box to the window
        Add(box);

        // Get the screen size and assign values to variables
        Gdk.Screen screen = Gdk.Screen.Default;
        int scWidth = screen.Width;
        int scHeight = screen.Height;

        //assign the thickness to be 450 pixels
        int panelWidth = 450;

        //assign the height of the panel to be the whole length of the user's screen
        SetDefaultSize(panelWidth, scHeight);
        SetSizeRequest(panelWidth, scHeight);

        //moving panel to side of screen
        Move(scWidth - panelWidth, 0);

        //shows everything
        ShowAll();
    }

    //changes the isotopes displayed to alpha
    private void AlphaButtonClicked(object sender, EventArgs e)
    {
        ShowIsotopes("Alpha");
    }

    //changes the isotopes displayed to beta
    private void BetaButtonClicked(object sender, EventArgs e)
    {
        ShowIsotopes("Beta");
    }

    //changes the isotopes displayed to gamma
    private void GammaButtonClicked(object sender, EventArgs e)
    {
        ShowIsotopes("Gamma");
    }

    //changes the isotopes displayed based on decay type
    private void ShowIsotopes(string decayType)
    {
        //removes the existing isotope radio buttons
        foreach (Widget child in IsotopeBox.Children)
        {
            IsotopeBox.Remove(child);
            child.Destroy();
        }

        string[] isotopes;

        //selects the isotopes for the chosen decay type
        switch (decayType)
        {
            case "Alpha":
                isotopes = new string[]
                {
                    "Uranium235",
                    "Radon222",
                    "Polonium210"
                };
                break;

            case "Beta":
                isotopes = new string[]
                {
                    "Carbon14",
                    "Tritium",
                    "Strontium90"
                };
                break;

            case "Gamma":
                isotopes = new string[]
                {
                    "Technetium99m",
                    "Barium137m",
                    "Indium115m"
                };
                break;

            default:
                return;
        }

        //creates the isotope radio buttons
        RadioButton FirstButton = null;

        foreach (string isotope in isotopes)
        {
            RadioButton IsotopeButton;

            //creates the first radio button
            if (FirstButton == null)
            {
                IsotopeButton = new RadioButton(isotope);
                FirstButton = IsotopeButton;
            }
            else
            {
                //links the radio button to the first button
                IsotopeButton = new RadioButton(FirstButton, isotope);
            }

            //stores the selected isotope
            IsotopeButton.Toggled += delegate
            {
                if (IsotopeButton.Active)
                {
                    selectedIsotope = isotope;
                }
            };

            //puts the isotope radio button into the box
            IsotopeBox.PackStart(IsotopeButton, false, false, 0);
        }

        //selects the first isotope by default
        FirstButton.Active = true;

        //updates the isotope display
        IsotopeBox.ShowAll();
    }

    public string Radiobuttonseleted()
    {
        //returns the currently selected isotope
        Console.WriteLine(selectedIsotope);
        return selectedIsotope;
    }

    //creates a function to retrieve the number of atoms chosen by the user
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

    //function linked to when the start simulation button is clicked
    private void OnButtonClicked(object sender, EventArgs e)
    {
        //runs the GetAtomNumber function
        int numberOfAtoms = Convert.ToInt32(GetAtomNumber());

        //gets the currently selected isotope
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
        // Uranium 235
        LinkedList<double> Uranium235 = new LinkedList<double>();

        Uranium235.Insert(Math.Log(2) / 703800000.0); // Decay constant
        Uranium235.Insert(1); // Alpha decay
        Uranium235.Insert(92); // Atomic number

        // Radon 222
        LinkedList<double> Radon222 = new LinkedList<double>();

        Radon222.Insert(Math.Log(2) / (3.82 / 365.25));
        Radon222.Insert(1);
        Radon222.Insert(86);

        // Polonium 210
        LinkedList<double> Polonium210 = new LinkedList<double>();

        Polonium210.Insert(Math.Log(2) / (138.376 / 365.25));
        Polonium210.Insert(1);
        Polonium210.Insert(84);

        // Carbon 14
        LinkedList<double> Carbon14 = new LinkedList<double>();

        Carbon14.Insert(Math.Log(2) / 5730.0);
        Carbon14.Insert(2); // Beta decay
        Carbon14.Insert(6);

        // Tritium
        LinkedList<double> Tritium = new LinkedList<double>();

        Tritium.Insert(Math.Log(2) / 12.32);
        Tritium.Insert(2);
        Tritium.Insert(1);

        // Strontium 90
        LinkedList<double> Strontium90 = new LinkedList<double>();

        Strontium90.Insert(Math.Log(2) / 28.79);
        Strontium90.Insert(2);
        Strontium90.Insert(38);

        // Technetium 99m
        LinkedList<double> Technetium99m = new LinkedList<double>();

        Technetium99m.Insert(Math.Log(2) / (6.0067 / 24 / 365.25));
        Technetium99m.Insert(3); // Gamma decay
        Technetium99m.Insert(43);

        // Barium 137m
        LinkedList<double> Barium137m = new LinkedList<double>();

        Barium137m.Insert(Math.Log(2) / (2.552 / 60 / 24 / 365.25));
        Barium137m.Insert(3);
        Barium137m.Insert(56);

        // Indium 115m
        LinkedList<double> Indium115m = new LinkedList<double>();

        Indium115m.Insert(Math.Log(2) / (4.486 / 24 / 365.25));
        Indium115m.Insert(3);
        Indium115m.Insert(49);

        // Return the list for the selected atom
        switch (passedAtomname)
        {
            case "Uranium235":
                return Uranium235;

            case "Radon222":
                return Radon222;

            case "Polonium210":
                return Polonium210;

            case "Carbon14":
                return Carbon14;

            case "Tritium":
                return Tritium;

            case "Strontium90":
                return Strontium90;

            case "Technetium99m":
                return Technetium99m;

            case "Barium137m":
                return Barium137m;

            case "Indium115m":
                return Indium115m;
        }

        // Return null if the atom name wasn't found
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
            // Sets the default size
            SetDefaultSize(400, 300);

            // Creates a new colour
            Gdk.Color Black = new Gdk.Color();

            // Assigns hex code to colour
            Gdk.Color.Parse("#000000", ref Black);

            // Adds colour to Simulation window
            ModifyBg(StateType.Normal, Black);

            // Gets the screen size and assigns values to variables
            Gdk.Screen screen = Gdk.Screen.Default;
            int scWidth = screen.Width;
            int scHeight = screen.Height;

            int panelHeight = scHeight;

            // Sets the panel height to the full screen height
            SetDefaultSize((scWidth - 475), panelHeight);

            // Moves the panel to the side of the screen
            Move(0, 0);

            // Creates the main vertical box
            VBox MainBox = new VBox(false, 5);

            // Creates the drawing area for the particle simulation
            Gtk.DrawingArea drawingArea = new Gtk.DrawingArea();

            // Creates the drawing area for the graph
            Gtk.DrawingArea graphArea = new Gtk.DrawingArea();

            // Sets the graph height
            graphArea.SetSizeRequest(-1, 250);

            // Adds the particle simulation to the top
            MainBox.PackStart(drawingArea, true, true, 0);

            // Adds the graph underneath
            MainBox.PackStart(graphArea, false, true, 0);

            // Adds the main box to the simulation window
            Add(MainBox);

            // Connects the particle drawing area
            DecaySimulation.DrawingArea(drawingArea);

            // Connects the graph drawing area
            DecaySimulation.GraphDrawingArea(graphArea);

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

            LinkedNode<T> current = this.head;
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

            //stores how much the particle moves
            public float speedX;
            public float speedY;

            //random number generator
            static Random random = new Random();

            //creates a new particle
            public DecayParticle(float x, float y)
            {
                //sets the starting coordinates
                this.x = x;
                this.y = y;

                //generates a random angle
                double angle = random.NextDouble() * 2 * Math.PI;

                //sets the speed of the particle
                float speed = 3;

                //calculates the movement
                speedX = (float)(Math.Cos(angle) * speed);
                speedY = (float)(Math.Sin(angle) * speed);
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
        //stores the simulation data between timer updates
        static LinkedList<Coordinate> unDecayed;
        static LinkedList<Coordinate> allAtoms;
        static double decayConstant;
        static double deltaTime;
        static int AtomNumber;
        //creates a linked list to store all the particles released by decay
        static LinkedList<DecayParticle> DecayedParticels = new LinkedList<DecayParticle>();
        static bool simulationRunning = false;
        public static void Decaysimulation(int passedAtomNumber, double passedDecayConstant, double passedDeltaTime)
        {

            //creates a new linked list of coordinates to hold the undecayed atoms
            unDecayed = new LinkedList<Coordinate>();

            //fills the linked list with the number of atoms
            unDecayed = FillList(unDecayed, passedAtomNumber);

            //creates a list to store every atom for drawing
            allAtoms = new LinkedList<Coordinate>();

            //copies all the atoms into the drawing list
            LinkedNode<Coordinate> CurrentNode = unDecayed.head;

            while (CurrentNode != null)
            {
                allAtoms.Insert(CurrentNode.data);
                CurrentNode = CurrentNode.next;
            }

            //stops multiple simulations running at once
            if (simulationRunning || passedAtomNumber <= 0 || passedDeltaTime <= 0)
                return;

            //stores the simulation settings
            AtomNumber = passedAtomNumber;
            decayConstant = passedDecayConstant;
            deltaTime = passedDeltaTime;

            simulationRunning = true;

            //passes the full atom list to the drawing area
            drawingAtoms = allAtoms;

            //starts the drawing timer
            StartDrawing();

            //starts the simulation timer
            GLib.Timeout.Add(1000, SimulationTick);

            GraphData = new LinkedList<GraphPoint>();

            elapsedTime = 0;
            initialAtomNumber = passedAtomNumber;
            estimatedHalfLife = 0;

            // Adds the starting point to the graph
            GraphData.Insert(new GraphPoint(0, passedAtomNumber));

            // Calculates the estimated half-life
            CalculateEstimatedHalfLife();

            // Updates the graph
            if (graphDrawingArea != null)
                graphDrawingArea.QueueDraw();

        }
        static bool SimulationTick()
        {

            Console.WriteLine("Number of undecayed atoms: " + AtomNumber);

            unDecayed.DisplayLL(unDecayed);

            //runs the monte carlo simulation round
            //and returns the new list of undecayed atoms
            unDecayed = RunMonteCarloRound(unDecayed, unDecayed.head, decayConstant, deltaTime);

            //updates the drawing list with decayed atoms
            UpdateDrawingList(allAtoms, unDecayed);

            //sweeps the list to remove any decayed atoms
            unDecayed = SweepList(unDecayed);

            AtomNumber = unDecayed.Length(unDecayed);

            // Updates the elapsed simulation time
            elapsedTime += deltaTime;

            // Adds the latest reading to the graph
            GraphData.Insert(new GraphPoint(elapsedTime, AtomNumber));

            // Requests the graph to redraw
            if (graphDrawingArea != null)
                graphDrawingArea.QueueDraw();

            unDecayed.DisplayLL(unDecayed);

            //stops the simulation when no atoms remain
            if (AtomNumber == 0)
            {
                simulationRunning = false;
                return false;
            }

            return true;
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
        public static void DrawDecay(Cairo.Context cr)
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
        public static void DrawingArea(Gtk.DrawingArea passedDrawingArea)
        {
            //stores the drawing area from the simulation window
            drawingArea = passedDrawingArea;

            //links the drawing function to the drawing area
            drawingArea.Drawn += Draw;
        }

        static void Draw(object sender, Gtk.DrawnArgs args)
        {
            //i dont know why this is needed but it is
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
            GLib.Timeout.Add(16, DrawingTimer);
        }

        //requests GTK to draw the next frame
        private static bool DrawingTimer()
        {
            if (drawingArea != null)
                drawingArea.QueueDraw();

            //keeps the drawing timer running
            return true;
        }
        // Connects the graph drawing area
        public static void GraphDrawingArea(Gtk.DrawingArea passedGraphArea)
        {
            graphDrawingArea = passedGraphArea;

            // Links the graph drawing function
            graphDrawingArea.Drawn += DrawGraph;
        }

        // Draws the graph
        static void DrawGraph(object sender, Gtk.DrawnArgs args)
        {
            Cairo.Context cGraph = args.Cr;

            int width = graphDrawingArea.AllocatedWidth;
            int height = graphDrawingArea.AllocatedHeight;

            // Sets the background to black
            cGraph.SetSourceRGB(0, 0, 0);
            cGraph.Paint();

            // Sets the graph line colour to cyan
            cGraph.SetSourceRGB(0, 1, 1);
            cGraph.LineWidth = 2;

            // Sets graph margins
            double left = 50;
            double right = width - 20;
            double top = 20;
            double bottom = height - 35;

            // Draws the axes
            cGraph.SetSourceRGB(1, 1, 1);
            cGraph.LineWidth = 1;

            cGraph.MoveTo(left, top);
            cGraph.LineTo(left, bottom);
            cGraph.LineTo(right, bottom);
            cGraph.Stroke();

            if (GraphData == null || GraphData.head == null)
                return;

            // Finds the maximum time
            double maxTime = elapsedTime;

            if (maxTime <= 0)
                maxTime = 1;

            // Finds the maximum atom count
            double maxAtoms = initialAtomNumber;

            if (maxAtoms <= 0)
                maxAtoms = 1;

            // Starts at the first graph point
            LinkedNode<GraphPoint> CurrentNode = GraphData.head;

            bool firstPoint = true;

            // Draws the graph line
            cGraph.SetSourceRGB(0, 1, 1);
            cGraph.LineWidth = 2;

            while (CurrentNode != null)
            {
                GraphPoint point = CurrentNode.data;

                // Converts the data into screen coordinates
                double x = left + (point.Time / maxTime) * (right - left);
                double y = bottom - ((double)point.Atoms / maxAtoms) * (bottom - top);

                if (firstPoint)
                {
                    cGraph.MoveTo(x, y);
                    firstPoint = false;
                }
                else
                {
                    cGraph.LineTo(x, y);
                }

                CurrentNode = CurrentNode.next;
            }
            // Displays the estimated half-life
            cGraph.SetSourceRGB(1, 1, 1);
            cGraph.SetFontSize(12);

            if (estimatedHalfLife > 0)
            {
                cGraph.MoveTo(left, height - 10);
                cGraph.ShowText("Estimated half-life: " +
                    estimatedHalfLife.ToString("F2") + " years");
            }
            else
            {
                cGraph.MoveTo(left, height - 10);
                cGraph.ShowText("Estimated half-life: Calculating...");
            }
            cGraph.Stroke();

            // Draws the graph title
            cGraph.SetSourceRGB(1, 1, 1);
            cGraph.SetFontSize(14);
            cGraph.MoveTo(left, 15);
            cGraph.ShowText("Radioactive Decay");
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
            DrawDecay(cr);

        }
        // Stores the points used to draw the graph
        static LinkedList<GraphPoint> GraphData = new LinkedList<GraphPoint>();

        // Stores the elapsed simulation time
        static double elapsedTime = 0;

        // Stores the starting number of atoms
        static int initialAtomNumber;

        // Stores the estimated half-life
        static double estimatedHalfLife = 0;
        // Stores the graph drawing area
        static Gtk.DrawingArea graphDrawingArea;

        // Calculates the estimated half-life from the graph data
        static void CalculateEstimatedHalfLife()
        {
            // Calculates half of the starting atom count
            double halfAtoms = initialAtomNumber / 2.0;

            LinkedNode<GraphPoint> CurrentNode = GraphData.head;

            // Moves through the graph readings
            while (CurrentNode != null && CurrentNode.next != null)
            {
                GraphPoint PreviousPoint = CurrentNode.data;
                GraphPoint NextPoint = CurrentNode.next.data;

                // Checks if the atom count has crossed the half-life point
                if (PreviousPoint.Atoms > halfAtoms &&
                    NextPoint.Atoms <= halfAtoms)
                {
                    // Calculates how far between the two readings
                    double fraction = (PreviousPoint.Atoms - halfAtoms) /
                        (PreviousPoint.Atoms - NextPoint.Atoms);

                    // Interpolates the time between the readings
                    estimatedHalfLife = PreviousPoint.Time +
                        fraction * (NextPoint.Time - PreviousPoint.Time);

                    return;
                }

                CurrentNode = CurrentNode.next;
            }
        }
    }

    public class GraphPoint
    {
        public double Time;
        public int Atoms;

        public GraphPoint(double time, int atoms)
        {
            Time = time;
            Atoms = atoms;
        }

    }
}

