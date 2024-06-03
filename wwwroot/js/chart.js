let myChart = null;

async function drawChart() {
    var stationId = document.getElementById("stationSelect").value;

    try {
        var response = await fetch(`/api/HistoryEntries?StationID=${stationId}`);

        if (!response.ok) {
            console.error("Server response wasn't OK. Status:", response.status, response.statusText);
            return;
        }

        var responseData = await response.json();

        if (stationId === "0") {
            // Handle combined data for all stations
            var datasets = responseData.map(stationData => {
                if (stationData.Entries) {
                    var timestamps = stationData.Entries.map(entry => new Date(entry.timestamp));
                    var values = stationData.Entries.map(entry => entry.value);

                    return {
                        label: stationData.StationName,
                        data: values.map((value, index) => ({ x: timestamps[index], y: value })),
                        borderColor: getRandomColor(),
                        borderWidth: 1,
                        fill: false
                    };
                }
            });

            if (myChart) {
                myChart.data.labels = datasets[0].data.map(d => d.x);
                myChart.data.datasets = datasets;
                myChart.update();
            } else {
                var ctx = document.getElementById('myChart').getContext('2d');
                myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        datasets: datasets
                    },
                    options: {
                        scales: {
                            x: {
                                type: 'time',
                                time: {
                                    unit: 'minute',
                                    displayFormats: {
                                        minute: 'MMM dd, HH:mm',
                                    }
                                },
                                title: {
                                    display: true,
                                    text: 'Date'
                                }
                            },
                            y: {
                                beginAtZero: true,
                                title: {
                                    display: true,
                                    text: 'Surface Level'
                                }
                            }
                        }
                    }
                });
            }
        } else {
            // Handle data for a single station
            var timestamps = responseData.map(entry => new Date(entry.timestamp));
            var values = responseData.map(entry => entry.value);

            if (myChart) {
                myChart.data.labels = timestamps;
                myChart.data.datasets = [{
                    label: 'Surface Level',
                    data: values.map((value, index) => ({ x: timestamps[index], y: value })),
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                    borderColor: 'rgba(255, 99, 132, 1)',
                    borderWidth: 1,
                    fill: false
                }];
                myChart.update();
            } else {
                var ctx = document.getElementById('myChart').getContext('2d');
                myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: timestamps,
                        datasets: [{
                            label: 'Surface Level',
                            data: values.map((value, index) => ({ x: timestamps[index], y: value })),
                            backgroundColor: 'rgba(255, 99, 132, 0.2)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 1,
                            fill: false
                        }]
                    },
                    options: {
                        scales: {
                            x: {
                                type: 'time',
                                time: {
                                    unit: 'minute',
                                    displayFormats: {
                                        minute: 'MMM dd, HH:mm',
                                    }
                                },
                                title: {
                                    display: true,
                                    text: 'Date'
                                }
                            },
                            y: {
                                beginAtZero: true,
                                title: {
                                    display: true,
                                    text: 'Surface Level'
                                }
                            }
                        }
                    }
                });
            }
        }
    } catch (error) {
        console.error("An error occurred while fetching data:", error);
    }
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

// Add an event listener to refresh the chart when a new station is selected
document.getElementById("stationSelect").addEventListener("change", drawChart);

// Initial chart load
document.addEventListener("DOMContentLoaded", drawChart);
