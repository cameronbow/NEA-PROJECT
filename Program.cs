using Gtk;
using Pango;
using SDL3;


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
    public SelectionBox() : base("Selection Box")
    {



        SetDefaultSize(400, 300);


        //Creates a new colour
        Gdk.Color Gray = new Gdk.Color();

        //assigns hex code to colour
        Gdk.Color.Parse("#808080", ref Gray);

        //adds colour to selction box
        ModifyBg(StateType.Normal, Gray);

        //creates the slider lable
        Label sliderLabel = new Label("Slider:");

        //creates "vertical box" but think about it as a virtual box that each smaller component can be placed in
        VBox box = new VBox(false, 10);

        //sets the size of each virtical box
        box.BorderWidth = 20;

        //creates the actual slider going from 0 to 100 in incriments of 1
        Gtk.Scale slider = new Gtk.Scale( Orientation.Horizontal,0,100,1);

        //slider inital value
        slider.Value = 50;

        //puts the the lable in the first virtical box
        box.PackStart(sliderLabel, false, false, 0);

        //puts the slider underneath in the second vertical box
        box.PackStart(slider, false, false, 0);



        //creates first option
        RadioButton option1 = new RadioButton("Option 1");

        //creates second option linking with the first
        RadioButton option2 = new RadioButton(option1, "Option 2");

        //creates third option linking with the first
        RadioButton option3 = new RadioButton(option1, "Option 3");

        //has the first option selcted by defult
        option1.Active = true;

        //puts all buttons under lable and slider and then in order
        box.PackStart(option1, false, false, 0);
        box.PackStart(option2, false, false, 0);
        box.PackStart(option3, false, false, 0);

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
}
public class Simulationbox : Window
{
    public Simulationbox() : base("Simulation")
    {


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
        SetDefaultSize((scWidth-475), panelHeight);
        SetSizeRequest((scWidth-475), panelHeight);

        //moving panel to side of screen
        Move(0, screen.Height-50);

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
        SelectionBox selectionbox = new SelectionBox();

        // Create the simulation window
        Simulationbox simulation = new Simulationbox();

        // Show both windows
        background.Show();
        selectionbox.Show();
        simulation.Show();

        Application.Run();
    }
}