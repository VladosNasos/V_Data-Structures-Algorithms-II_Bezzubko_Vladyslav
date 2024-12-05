using EastBulgariaPathFinderWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EastBulgariaPathFinderWPF
{
    public partial class MainWindow : Window
    {
        private readonly IGraph _graph;
        private readonly IPathFinder _pathFinder;

        public MainWindow()
        {
            InitializeComponent();
            _graph = InitializeGraph();
            _pathFinder = new PathFinder(_graph);
            PopulateComboBoxes();
        }

        private void PopulateComboBoxes()
        {
            var cities = _graph.GetCities().Select(c => c.Name).OrderBy(name => name).ToList();
            StartCityComboBox.ItemsSource = cities;
            EndCityComboBox.ItemsSource = cities;
        }

        private void FindPathButton_Click(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "";
            PathDisplayPanel.Children.Clear();

            string startCityName = StartCityComboBox.SelectedItem as string;
            string endCityName = EndCityComboBox.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(startCityName) || string.IsNullOrWhiteSpace(endCityName))
            {
                StatusTextBlock.Text = "Please select both Start and End cities.";
                return;
            }

            if (startCityName.Equals(endCityName, StringComparison.OrdinalIgnoreCase))
            {
                StatusTextBlock.Text = "Start and End cities cannot be the same.";
                return;
            }

            try
            {
                var startCity = _graph.GetCityByName(startCityName);
                var endCity = _graph.GetCityByName(endCityName);

                var result = _pathFinder.FindQuickestPath(startCity, endCity);

                DisplayPath(result);
            }
            catch (KeyNotFoundException knfEx)
            {
                StatusTextBlock.Text = $"Error: {knfEx.Message}";
            }
            catch (InvalidOperationException invOpEx)
            {
                StatusTextBlock.Text = $"Error: {invOpEx.Message}";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"An unexpected error occurred: {ex.Message}";
            }
        }

        private void DisplayPath(PathResult result)
        {
            if (result.Path == null || result.Path.Count == 0)
            {
                StatusTextBlock.Text = "No path found.";
                return;
            }

            // Display each step with distance
            for (int i = 0; i < result.Path.Count - 1; i++)
            {
                var from = result.Path[i].Name;
                var to = result.Path[i + 1].Name;
                var road = _graph.GetRoadsFrom(result.Path[i]).FirstOrDefault(r => r.To.Name.Equals(to, StringComparison.OrdinalIgnoreCase));

                if (road != null)
                {
                    var travelTime = Math.Round(road.DistanceKm / road.MaxSpeedKmh, 2);
                    TextBlock step = new TextBlock
                    {
                        Text = $"{from} -> {to} | Distance: {road.DistanceKm} km | Time: {travelTime} hrs",
                        Margin = new Thickness(0, 2, 0, 2)
                    };
                    PathDisplayPanel.Children.Add(step);
                }
            }

            // Display total distance and time
            TextBlock total = new TextBlock
            {
                Text = $"Total Distance: {result.TotalDistanceKm} km | Total Travel Time: {result.TotalTimeHours} hrs",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 0)
            };
            PathDisplayPanel.Children.Add(total);
        }

        private IGraph InitializeGraph()
        {
            var graph = new Graph();

            // Define Cities
            var cities = new List<ICity>
            {
                new City("Varna"),
                new City("Burgas"),
                new City("Dobrich"),
                new City("Silistra"),
                new City("Razgrad"),
                new City("Tyrgovishte"),
                new City("Shumen"),
                new City("Veliko Tarnovo"),
                new City("Sliven"),
                new City("Yambol"),
                new City("Kazanlak"),
                new City("Stara Zagora")
            };

            foreach (var city in cities)
            {
                graph.AddCity(city);
            }

            // Data Source: Approximate real-world distances and standard speed limits for first-class roads in Bulgaria

            var roads = new List<IRoad>
            {
                // Varna Connections
                new Road(cities[0], cities[2], 80, 100),    // Varna - Dobrich
                new Road(cities[0], cities[6], 160, 100),   // Varna - Shumen
                new Road(cities[0], cities[1], 220, 100),   // Varna - Burgas

                // Dobrich Connections
                new Road(cities[2], cities[3], 70, 100),    // Dobrich - Silistra

                // Silistra Connections
                new Road(cities[3], cities[4], 70, 100),    // Silistra - Razgrad

                // Razgrad Connections
                new Road(cities[4], cities[11], 120, 100),  // Razgrad - Stara Zagora
                new Road(cities[4], cities[5], 100, 100),   // Razgrad - Tyrgovishte
                new Road(cities[4], cities[6], 150, 100),   // Razgrad - Shumen

                // Stara Zagora Connections
                new Road(cities[11], cities[1], 150, 100),  // Stara Zagora - Burgas
                new Road(cities[11], cities[10], 50, 100),  // Stara Zagora - Kazanlak

                // Shumen Connections
                new Road(cities[6], cities[7], 120, 100),   // Shumen - Veliko Tarnovo

                // Veliko Tarnovo Connections
                new Road(cities[7], cities[8], 140, 100),   // Veliko Tarnovo - Sliven
                new Road(cities[7], cities[5], 100, 100),   // Veliko Tarnovo - Tyrgovishte

                // Sliven Connections
                new Road(cities[8], cities[9], 100, 100),   // Sliven - Yambol

                // Yambol Connections
                new Road(cities[9], cities[10], 100, 100),  // Yambol - Kazanlak

                // Kazanlak Connections
                // Already connected to Stara Zagora


            };

            foreach (var road in roads)
            {
                graph.AddRoad(road);
            }

            return graph;
        }
    }
}
