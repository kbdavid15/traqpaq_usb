﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace traqpaqWPF
{

    /// <summary>
    /// Interaction logic for RecordTablePage.xaml
    /// </summary>
    public partial class RecordTablePage : Page
    {
        ObservableCollection<Record> _RecordTable = new ObservableCollection<Record>();
        public ObservableCollection<Record> RecordTable { get { return _RecordTable; } }
        MainWindow main;

        // create the dummy laps
        List<double> latitudes1 = new List<double> { 42.1838786, 42.17332503, 42.1607474, 42.15192871, 
                                                     42.14788076, 42.1538081, 42.16493994, 42.17462615,
                                                     42.18286665, 42.1838786, 42.1838786};
        List<double> longitudes1 = new List<double> { -83.32956822, -83.34756995, -83.35109965, -83.34209883,
                                                      -83.32321473, -83.31368443, -83.30027137, -83.30556597,
                                                      -83.3188025, -83.32939178, -83.32939178 };
        List<double> latitudes2 = new List<double> { 42.1848786, 42.17432503, 42.1617474, 42.15292871, 
                                                     42.14888076, 42.1548081, 42.16593994, 42.17562615,
                                                     42.18386665, 42.1848786, 42.1848786};
        List<double> longitudes2 = new List<double> { -83.32856822, -83.34656995, -83.35009965, -83.34109883,
                                                      -83.32221473, -83.31268443, -83.30027137, -83.30456597,
                                                      -83.3198025, -83.32839178, -83.32839178 };
        List<double> flintLat1 = new List<double> { 42.9015819914639, 42.90179883129895, 42.90114822797477, 42.89811230264604, 42.89507629349828, 42.892040368169546, 42.892040368169546, 42.88835383951664, 42.88835383951664, 42.88379986770451, 42.885534754022956, 42.88748639635742, 42.89008864201605, 42.88943812251091, 42.88965496234596, 42.892257208004594, 42.892257208004594, 42.892257208004594, 42.89507629349828, 42.89507629349828, 42.89312465116382, 42.89073924534023, 42.88726955652237, 42.88271558471024, 42.880113339051604, 42.875776290893555, 42.87317396141589, 42.870138036087155, 42.86862007342279, 42.86818630993366, 42.86883691325784, 42.86948743276298, 42.87100547924638, 42.87187283858657, 42.87382456474006, 42.87620997056365, 42.879029056057334, 42.88163130171597, 42.885751593858004, 42.88835383951664, 42.8918235283345, 42.896594339981675, 42.89919658564031, 42.90244935080409, 42.905268520116806, 42.90830444544554, 42.91264157742262, 42.91611126624048, 42.919797794893384, 42.92261688038707, 42.92284369468689, 42.92205420322716, 42.92023147456348, 42.92023147456348, 42.9158943425864, 42.91220781393349, 42.90895504876971, 42.906135879457, 42.902015671133995, 42.89832914248109, 42.89529321715236, 42.89399201050401, 42.8903054818511, 42.887703236192465, 42.88900435902178, 42.892040368169546, 42.892040368169546, 42.88922128267586, 42.88705271668732, 42.88314934819937, 42.881197622045875, 42.877727933228016, 42.87534252740443, 42.87620997056365, 42.879029056057334, 42.88249874487519, 42.885534754022956, 42.89073924534023, 42.89399201050401, 42.89702801965177, 42.902015671133995, 42.905485359951854, 42.905485359951854, 42.90895504876971, 42.91481014341116, 42.91611126624048, 42.91762922890484, 42.92196627706289, 42.92196627706289, 42.92608656920493, 42.9273381549865, 42.92652024887502, 42.925002286210656, 42.92370116338134, 42.920882077887654, 42.920882077887654, 42.91567750275135, 42.91567750275135, 42.91047301143408, 42.91047301143408, 42.907653925940394, 42.907653925940394, 42.904184237122536, 42.904184237122536, 42.90179883129895, 42.90179883129895, 42.90136515162885, 42.90136515162885, 42.9015819914639 };
        List<double> flintLong1 = new List<double> { -83.78953001461923, -83.79694246686995, -83.80170757882297, -83.80356073379517, -83.80382543429732, -83.80303124897182, -83.80303124897182, -83.79932502284646, -83.79932502284646, -83.7913830857724, -83.78555900417268, -83.78158807754517, -83.78450011834502, -83.78820634447038, -83.79350094124675, -83.79455982707441, -83.79455982707441, -83.79455982707441, -83.78926523029804, -83.78502951934934, -83.78026440739632, -83.77655818127096, -83.77470502629876, -83.7762934807688, -83.78370593301952, -83.78423533402383, -83.78185277804732, -83.7778818514198, -83.7728519551456, -83.76800537109375, -83.75988012179732, -83.75299715436995, -83.74611410312355, -83.74053955078125, -83.73552490957081, -83.73208338394761, -83.7297008279711, -83.72917134314775, -83.72917134314775, -83.72996552847326, -83.73075971379876, -83.7315539829433, -83.7304950132966, -83.7281124573201, -83.7254651170224, -83.72043522074819, -83.71514062397182, -83.71434643864632, -83.71461113914847, -83.71858214959502, -83.72595277614892, -83.72601857408881, -83.7315539829433, -83.7315539829433, -83.73526020906866, -83.73870165087283, -83.74161369167268, -83.74240787699819, -83.74399624764919, -83.74452573247254, -83.74929092824459, -83.75670338049531, -83.75829175114632, -83.75379133969545, -83.74770255759358, -83.74053955078125, -83.74053955078125, -83.74053955078125, -83.73711328022182, -83.74053955078125, -83.74399624764919, -83.75299715436995, -83.75882123596966, -83.76438053324819, -83.76800537109375, -83.76800537109375, -83.76800537109375, -83.76800537109375, -83.76438053324819, -83.76226267777383, -83.75855645164847, -83.75961542129517, -83.75961542129517, -83.76358634792268, -83.76199797727168, -83.7543208245188, -83.74796725809574, -83.7474377732724, -83.7474377732724, -83.75643859617412, -83.76870735548437, -83.7728519551456, -83.7762934807688, -83.78052910789847, -83.78238226287067, -83.78238226287067, -83.78450011834502, -83.78450011834502, -83.78317644819617, -83.78317644819617, -83.77947022207081, -83.77947022207081, -83.77655818127096, -83.77655818127096, -83.77973492257297, -83.77973492257297, -83.78450011834502, -83.78450011834502, -83.78953001461923 };
        List<double> flintLat2 = new List<double> { 42.902367264808305, 42.90228928056689, 42.90186892157434, 42.89818023241232, 42.89541648224031, 42.89295113428955, 42.89276042286497, 42.88882092863957, 42.88859968531931, 42.884042833110996, 42.88606944237792, 42.88818963371989, 42.89080664861361, 42.89017481961525, 42.89041229033883, 42.89314100354533, 42.89247642161561, 42.89309052959788, 42.89587974385741, 42.89522790108595, 42.89369107948109, 42.8916499016621, 42.888065834644706, 42.88340505751816, 42.880654653422894, 42.87586088539462, 42.87365745484865, 42.870241277550186, 42.868959892225135, 42.868396117073665, 42.86891819582499, 42.86982071444566, 42.87179915821037, 42.87238480461792, 42.87437427176019, 42.87645246274142, 42.87959323882444, 42.88246863804293, 42.88668104370436, 42.88884034383258, 42.891981351135406, 42.89707239780095, 42.89970129488506, 42.90280841074719, 42.90537665510542, 42.909237134947496, 42.91295784407914, 42.91645961212541, 42.919903309871266, 42.922998696114526, 42.92292037682172, 42.92257917820107, 42.92122647518181, 42.920671122666874, 42.9167515759326, 42.91292641907561, 42.90974098372614, 42.90638925275726, 42.90229272239525, 42.89924157032739, 42.89613776468138, 42.89430018846748, 42.89068445125413, 42.887877586481125, 42.88994714223147, 42.89247482325699, 42.892069611378005, 42.890117315398214, 42.887982608686755, 42.88382946557368, 42.88130179927436, 42.87839229807572, 42.87537186887308, 42.8767480796549, 42.87918381308319, 42.88314114250918, 42.885604570166464, 42.891063526146496, 42.89494692153213, 42.897589134404605, 42.902711512264815, 42.90582791130186, 42.90559416042266, 42.90921713017877, 42.91518761652031, 42.91699624831911, 42.918298020608894, 42.92219945576848, 42.92254234107055, 42.92684411812344, 42.92832539676758, 42.926958231028685, 42.925650706432435, 42.92385049416293, 42.92135979703974, 42.92132080629216, 42.91595973541379, 42.91659238448507, 42.91098490196616, 42.91050360339327, 42.90795985246703, 42.90777741116479, 42.90454754883501, 42.90501416843634, 42.90191832165113, 42.90212890740721, 42.90163407518848, 42.90164370847541, 42.90184970081744 };
        List<double> flintLong2 = new List<double> { -83.78915044647611, -83.7969236499334, -83.80133982274644, -83.80272132865798, -83.80325316990725, -83.80302172655396, -83.80213190366983, -83.7992343089266, -83.79930979844012, -83.79047934657908, -83.78487629575272, -83.78115490645513, -83.78384212252035, -83.78816529572647, -83.79306273714717, -83.7939881158589, -83.79441202779498, -83.79435758399983, -83.78893678536903, -83.78460521343615, -83.7795503234651, -83.7761082195225, -83.77463991164777, -83.7753005554254, -83.78354834082752, -83.78358747147101, -83.78137267724516, -83.77731273390629, -83.77202632169534, -83.7674334084234, -83.75961705783479, -83.75249104142877, -83.74605987793569, -83.73956298434058, -83.73477912158796, -83.73187782620934, -83.72875136480516, -83.7285798567347, -83.7283597776921, -83.72980743243471, -83.73009441604363, -83.73079172786441, -83.73027401760496, -83.72723086855623, -83.7246619184945, -83.72032421979182, -83.71451751142094, -83.71366564518063, -83.71415684313983, -83.71841036471587, -83.72579324481949, -83.72519565106766, -83.73130673458083, -83.7311649907879, -83.73487958325016, -83.73813031889976, -83.74124451737248, -83.74166597842249, -83.7430101028924, -83.74452042853447, -83.74922657409776, -83.75622184466766, -83.75808618120527, -83.75351509729711, -83.74674918979021, -83.7399530699439, -83.73970138248906, -83.74008979212606, -83.736929624821, -83.74017019440606, -83.74350900488078, -83.75294643197627, -83.75815961031915, -83.76342170097566, -83.76712127726397, -83.76701866584564, -83.76717698371809, -83.7678777152303, -83.76418928165172, -83.7614373859542, -83.75812425638057, -83.75908757922808, -83.75934970776527, -83.76349119609269, -83.76107577590007, -83.75362666706529, -83.74767884685289, -83.74663133871596, -83.74688963467581, -83.75569092022998, -83.7685169321146, -83.77195965214064, -83.77551181485549, -83.78030919581364, -83.78150785204735, -83.78148024345573, -83.78429344485397, -83.78364810185477, -83.78303010966071, -83.78262733779044, -83.77940858798766, -83.77917051377895, -83.77583736765216, -83.77639502010663, -83.77880848133417, -83.7791292813669, -83.78429712558726, -83.78441307039895, -83.78884598792413 };
        List<double> flintLat3 = new List<double> { 42.90229202358746, 42.90250606987573, 42.901429123581075, 42.898238268219515, 42.89598261845265, 42.8927077958289, 42.89257630204375, 42.88904710432825, 42.88909698183525, 42.88465489998286, 42.886272965979515, 42.88809665845426, 42.89024338403804, 42.88972216501615, 42.89014667606151, 42.89294510847255, 42.892340528623194, 42.89245697584053, 42.89530297717284, 42.895114271090485, 42.89358043865681, 42.891246175626414, 42.88813645611482, 42.88359265946336, 42.880303670977405, 42.876635372688824, 42.87362331820999, 42.870392882967884, 42.86936954022466, 42.868663925224524, 42.868891043682495, 42.869814043476154, 42.871573452056964, 42.87221856349534, 42.874416781741665, 42.876798436060696, 42.87974162760435, 42.88254866692856, 42.88632615544216, 42.889278643579686, 42.89195063645045, 42.89677289644589, 42.899321279045296, 42.90343631333605, 42.905487529567665, 42.90864993535309, 42.91359882600923, 42.91710904370961, 42.91994152558963, 42.92312974956793, 42.923036821831914, 42.92287239601901, 42.92096546511883, 42.920425743030236, 42.916258955642945, 42.91230033363055, 42.90937582071532, 42.90650384599918, 42.902139535541764, 42.89838528521643, 42.895823828056486, 42.894270689951604, 42.89030969685288, 42.888695249833994, 42.88970854754669, 42.892913823718736, 42.892391714106395, 42.88945015256453, 42.88755701000487, 42.88375892747053, 42.8818095382328, 42.877849108280586, 42.87613831096921, 42.876896002698906, 42.87946212476325, 42.882761575409496, 42.8864148359194, 42.89136046503737, 42.894494070337394, 42.8978174688801, 42.90249627092182, 42.90609516151958, 42.906404020884324, 42.90968578989705, 42.91497701636115, 42.91672005783812, 42.918333377183146, 42.92213505325783, 42.922028539333, 42.926508026858, 42.927834476464376, 42.92696936065597, 42.9256080347142, 42.9244536572553, 42.92140908666346, 42.92145070688272, 42.91596837829105, 42.91597073982138, 42.911378525884146, 42.911397655305656, 42.90768888091722, 42.908032583966765, 42.90454045304199, 42.90512984134621, 42.90227892769581, 42.902626961510016, 42.90198249940199, 42.902086514928406, 42.90237820193088 };
        List<double> flintLong3 = new List<double> { -83.78898764183883, -83.79633105897187, -83.80122105127411, -83.80341999771223, -83.80314190377403, -83.80259221605229, -83.80303077131363, -83.79926609451117, -83.7991556826371, -83.79050542692218, -83.78465682172356, -83.78086935339, -83.78425120837862, -83.78752756379797, -83.79334077509648, -83.79380856620273, -83.79450998556348, -83.79380054169953, -83.78864494260628, -83.78432088071366, -83.77972786565842, -83.77614013598446, -83.7746123014523, -83.77547544888316, -83.78309554647281, -83.78393611792964, -83.78087309308025, -83.77742372818444, -83.77191906286208, -83.76783707456362, -83.75939348521008, -83.75283808736138, -83.74590627624221, -83.74015665626176, -83.73549864402266, -83.7315715907715, -83.72882234858419, -83.72894202228579, -83.7287012049185, -83.72946841362102, -83.7300109088969, -83.73067647528656, -83.72996183320134, -83.72755085066879, -83.7248091259441, -83.71990450553665, -83.71490968441073, -83.71428726130428, -83.71442033584249, -83.71815631855368, -83.72592193807914, -83.72596939450197, -83.73122162627463, -83.73110046458223, -83.73507194003685, -83.73866879835985, -83.74137277843452, -83.74231174200555, -83.74367478628619, -83.74401034799983, -83.74870296916598, -83.75647757556723, -83.75755995522226, -83.75292619385625, -83.74734714640284, -83.7404029635621, -83.73956782363169, -83.73964696936709, -83.73681718276111, -83.74030743385822, -83.74377203866322, -83.75211275036256, -83.75852243491752, -83.76436810511596, -83.76710895334114, -83.76717483515888, -83.76777168749017, -83.7679067348055, -83.76357202356336, -83.76155604335064, -83.75774984263295, -83.75944271000031, -83.75902744074018, -83.76345922846498, -83.76179411978997, -83.75423459347374, -83.74756022049459, -83.74678647196674, -83.74681418736273, -83.75575212098342, -83.76773350681768, -83.77274593627521, -83.7759601820427, -83.78012445755597, -83.78185120774477, -83.78147768469228, -83.78370947521923, -83.78413773161513, -83.78260427300802, -83.78311832548596, -83.77888244638547, -83.77920947506249, -83.77626217195896, -83.77608404902006, -83.77883054310341, -83.77912615649095, -83.78401084840566, -83.78396632688352, -83.78859245256267 };


        public RecordTablePage(MainWindow main)
        {
            InitializeComponent();

            this.main = main;

            List<LapInfo> laps = new List<LapInfo>();
            laps.Add(new LapInfo { LapNo = "1", LapTime = "2:30", LapColor = Colors.LawnGreen, Latitudes = latitudes1, Longitudes = longitudes1, Track = "Airport" });
            laps.Add(new LapInfo { LapNo = "2", LapTime = "2:24", LapColor = Colors.Red, Latitudes = latitudes2, Longitudes = longitudes2, Track = "Airport" });
            List<LapInfo> flintLaps = new List<LapInfo>();
            flintLaps.Add(new LapInfo { LapNo = "1", LapTime = "6:32:13", LapColor = Colors.Honeydew, Latitudes = flintLat1, Longitudes = flintLong1, Track = "Flint" });
            flintLaps.Add(new LapInfo { LapNo = "2", LapTime = "6:36:42", LapColor = Colors.Indigo, Latitudes = flintLat2, Longitudes = flintLong2, Track = "Flint" });
            flintLaps.Add(new LapInfo { LapNo = "3", LapTime = "5:59:02", LapColor = Colors.NavajoWhite, Latitudes = flintLat3, Longitudes = flintLong3, Track = "Flint" });

            _RecordTable.Add(new Record("Airport", "8/30/2012", laps));
            _RecordTable.Add(new Record("Flint", "9/20/2012", flintLaps));
        }

        /// <summary>
        /// Double clicking an item in the listview shows the Data page and passes the item to it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listViewRecords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            while (obj != null && obj != listViewRecords)
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    goToDataPage(new Record[] { (Record)listViewRecords.SelectedItem });                    
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }            
        }

        /// <summary>
        /// When the selected record changes, update the canvas preview and the stats pane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listViewRecords_SelectionChanged(object sender, EventArgs e)
        {
            Record session = (Record)listViewRecords.SelectedItem;
            foreach (LapInfo lap in session.Laps)
            {
                Polyline polyline = new Polyline();
                polyline.Stroke = Brushes.Black;
                // find the smallest dimension, width or height, and use that to calculate the points
                int size;
                if (viewboxPreviewPane.RenderSize.Width > viewboxPreviewPane.RenderSize.Height)
                {
                    size = (int)viewboxPreviewPane.RenderSize.Height;
                }
                else
                {
                    size = (int)viewboxPreviewPane.RenderSize.Width;
                }
                double[] xCoord = NormalizeData(lap.Latitudes, 10, size - 10);
                double[] yCoord = NormalizeData(lap.Longitudes, 10, size - 10);
                for (int i = 0; i < xCoord.Length; i++)
                {
                    polyline.Points.Add(new Point(xCoord[i], yCoord[i]));
                }

                // Add the polyline to the viewbox
                viewboxPreviewPane.Child = polyline;
            }            
        }

        /// <summary>
        /// Normalize a set of data
        /// </summary>
        /// <param name="data">The enumerable data (double) to normalize</param>
        /// <param name="min">Min value of result</param>
        /// <param name="max">Max value of result</param>
        /// <returns>Array of doubles containing normalized data</returns>
        private static double[] NormalizeData(IEnumerable<double> data, int min, int max)
        {
            double dataMax = data.Max();
            double dataMin = data.Min();
            double range = dataMax - dataMin;

            return data
                .Select(d => (d - dataMin) / range)
                .Select(n => ((1 - n) * min + n * max))
                .ToArray();
        }

        private void buttonDataPage_Click(object sender, RoutedEventArgs e)
        {
            Record[] records = new Record[listViewRecords.SelectedItems.Count];
            for (int i = 0; i < listViewRecords.SelectedItems.Count; i++)
            {
                records[i] = (Record)listViewRecords.SelectedItems[i];
            }
            goToDataPage(records);
        }

        /// <summary>
        /// This function is called by the double click event and the datapage button click event
        /// </summary>
        /// <param name="records">Array of Record objects</param>
        private void goToDataPage(Record[] records)
        {
            // construct the record data page
            DataPage data = (DataPage)main.pages[(int)PageName.DATA];
            data.setRecord(records);
            // Navigate to the page
            main.navigatePage(PageName.DATA);
        }
    }
    /// <summary>
    /// Dummy class for adding info to the record
    /// </summary>
    public class LapInfo
    {
        public string LapNo { get; set; }
        public string LapTime { get; set; }
        public Color LapColor { get; set; }
        public List<double> Latitudes { get; set; }
        public List<double> Longitudes { get; set; }
        public bool AreAllChecked = false;
        public string Track { get; set; }
    }

    public class Record
    {
        public string trackName { get; set; }
        public string DateStamp { get; set; }
        public List<LapInfo> Laps { get; set; }
        public Record(string trackname, string date, List<LapInfo> laps)
        {
            trackName = trackname;
            DateStamp = date;
            Laps = laps;
        }
    }
}
