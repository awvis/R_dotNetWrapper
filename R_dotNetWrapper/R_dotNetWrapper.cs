using RDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace R_dotNetWrapper
{
    public class R_dotNetWrapper
    {
        REngine rEngine;

        public R_dotNetWrapper()
        {
            try
            {
                REngine.SetEnvironmentVariables();
                // initialising R engine

                this.rEngine = REngine.GetInstance();
                rEngine.Evaluate("require(dtw)");
                rEngine.Evaluate("require(SimilarityMeasures)");
                rEngine.Evaluate("require(vegan)");
                rEngine.Evaluate("sink(tempfile())");


            }
            catch (Exception)
            {
                Console.WriteLine("There was a problem in initializing R_Engine!");
            }

        }


        /// <summary>
        /// 2D Line plot with (x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="title"></param>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="type"></param>
        /// <param name="lty"></param>
        public void Plot2D(List<double> x, List<double> y, string title, string xAxis, string yAxis, PlotTypes type, PlotLineType lty)
        {
            // .NET Framework array to R vector.
            var xR = rEngine.CreateNumericVector(x.ToArray());
            var yR = rEngine.CreateNumericVector(y.ToArray());

            rEngine.SetSymbol("x", xR);
            rEngine.SetSymbol("y", yR);

            rEngine.Evaluate("plot(x,y, type = \"" + type.ToString() + "\",xlab =\"" + xAxis + "\", ylab =\"" + yAxis + "\", main =\"" + title + "\",lwd=2, lty = \"" + lty.ToString() + "\")");

        }

        public void SetWindowSize(int row, int col)
        {
            rEngine.Evaluate("par(mfrow=c (" + row.ToString() + "," + col.ToString() + "))");
        }

        /// <summary>
        /// 1D plot with list specify colors, title, plot type etc.,
        /// </summary>
        /// <param name="x"></param>
        /// <param name="title"></param>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="type"></param>
        /// <param name="ylim"></param>
        public void Plot1D(List<double> x, string title, string xAxis, string yAxis, PlotTypes type, string ylim)
        {
            // .NET Framework list to R vector.
            var xR = rEngine.CreateNumericVector(x.ToArray());


            rEngine.SetSymbol("x", xR);

            rEngine.Evaluate("plot(x, type = \"" + type.ToString() + "\", main =\"" + title + "\",xlab=\"" + xAxis.ToString() + "\",  ylab=\"" + yAxis.ToString() + "\", lwd=2, lty=1, col=rainbow(n=7), xlim=c(0,120), ylim= c(" + ylim.ToString() + ") )");

        }

        /// <summary>
        /// Saving the graph with path as input
        /// </summary>
        /// <param name="name"></param>
        public void SavePlot(string name)
        {
            rEngine.Evaluate("dev.copy(jpeg, file =\"" + name + "\" , width = 1920, height = 1080, units =\"px\" , pointsize = 12, quality = 100)");

        }
        /// <summary>
        /// Creating a Vector Field flow using arrows and orientation (x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="angle"></param>
        public void VectorFieldPlot(List<double> x, List<double> y, List<double> angle)
        {
            var xR = rEngine.CreateNumericVector(x.ToArray());
            var yR = rEngine.CreateNumericVector(y.ToArray());
            var aR = rEngine.CreateNumericVector(angle.ToArray());
            rEngine.SetSymbol("x", xR);
            rEngine.SetSymbol("y", yR);
            rEngine.SetSymbol("angle", aR);
            rEngine.Evaluate("plot(x,y, pch=19, cex=0.8, col=\"Blue\")");
            rEngine.Evaluate("length <- 0.2");
            rEngine.Evaluate("arrows(x, y, x1=x+length*cos(angle), y1=y+length*sin(angle), angle, length = 0.05, col = \"Gray\")");

        }
        /// <summary>
        /// Own command parsing through string value
        /// </summary>
        /// <param name="command"></param>
        public void OwnCommand(string command)
        {
            rEngine.Evaluate(command.ToString());
        }

        /// <summary>
        /// Adding an extra line to an existing 1D plot
        /// </summary>
        /// <param name="x"></param>
        /// <param name="color"></param>
        /// <param name="lty"></param>
        public void AddLine1D(List<double> x, string color, string lty)
        {
            var xR = rEngine.CreateNumericVector(x.ToArray());

            rEngine.SetSymbol("x", xR);
            rEngine.Evaluate("points(x , type=\"l\" , lty=" + lty.ToString() + " , lwd=2, col=\"" + color.ToString() + "\"  )");
        }

        /// <summary>
        /// Box plot 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="title"></param>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        public void PlotBoxplot(List<double> x, string title, string xAxis, string yAxis)
        {
            // .NET Framework array to R vector.
            var xR = rEngine.CreateNumericVector(x.ToArray());


            rEngine.SetSymbol("x", xR);

            rEngine.Evaluate("boxplot(x)");
        }

        
        public void Dispose()
        {
            rEngine.Dispose();
        }
    }
}

public enum PlotLineType
{
    twodash = 6,  /*for twodash,*/
    longdash = 5,
    dotdash = 4,
    dotted = 3,
    dashed = 2,
    solid = 1,
    blank = 0

}

public enum PlotTypes
{
    p, /*for points,*/

    l, /*for lines,*/

    b, /*for both,*/

    c, /*for the lines part alone of "b",*/

    o, /*for both ‘overplotted’,*/

    h, /*for ‘histogram’ like (or ‘high-density’) vertical lines,*/

    s, /*for stair steps,*/

    S, /*for other steps, see ‘Details’ below,*/

    n /*for no plotting.*/

}