using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Forms;

namespace Outlier.App
{
    public partial class Form1 : Form
    {
        #region constructor
        public Form1()
        {
            InitializeComponent();

            // Here we try to find the implementations to import from this assembly and 
            // the directory containing this assembly.
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory));
            this.container = new CompositionContainer(catalog);

            // Imports the dynmaic parts found using MEF
            this.container.ComposeParts(this);
        }
        #endregion

        #region event handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                // exits this application if not CSV file is specified
                Application.Exit();
                return;
            }

            var originalData = this.dataSource.GetData(this.openFileDialog1.FileName);
            var outliers = this.detector.GetOutliers(originalData);
            this.cleanData = this.detector.GetCleanData(originalData, outliers);

            // shows the outliers in the grid
            this.dataGridView1.DataSource = outliers;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                // does nothing if no file is specified
                return;
            }

            this.writer.WriteOutput(this.saveFileDialog1.FileName, cleanData);
            MessageBox.Show(
                "Done!", 
                "Exported", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }
        #endregion

        #region private variables
        /// <summary>
        /// The clean data with outliers removed from the source data.
        /// </summary>
        private List<PriceData> cleanData = null;

        /// <summary>
        /// The data source of price data points.
        /// </summary>
        /// <remarks>Note that attribute "Import" tells MEF to find an appropriate implementation dynamically 
        /// (which the implementation is not known at compile time).</remarks>
        [Import]
        private IDataSource dataSource = null;

        /// <summary>
        /// The outlier detector
        /// </summary>
        /// <remarks>Note that attribute "Import" tells MEF to find an appropriate implementation dynamically 
        /// (which the implementation is not known at compile time).</remarks>
        [Import]
        private IOutlierDetector detector = null;

        /// <summary>
        /// The data output writer
        /// </summary>
        /// <remarks>Note that attribute "Import" tells MEF to find an appropriate implementation dynamically 
        /// (which the implementation is not known at compile time).</remarks>
        [Import]
        private IOutputWriter writer = null;

        /// <summary>
        /// The composition container used by MEF to construct parts dynamically.
        /// </summary>
        private CompositionContainer container;
        #endregion
    }
}
