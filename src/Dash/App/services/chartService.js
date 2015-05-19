app.service('chartService', [function() {
    this.createExperienceSummaryChart = function(data) {
        google.load('visualization', '1.0', { 'packages': ['bar'], callback: drawChart });

        function drawChart() {
            var chartData = new google.visualization.DataTable();
            chartData.addColumn('string', 'Setup');
            chartData.addColumn('number', 'Successes');
            chartData.addColumn('number', 'Failures');
            chartData.addColumn('number', 'Success Ratio');

            data.forEach(function(item) {
                chartData.addRows([
                    [item.setup, item.successCount, item.failureCount, item.successRatio]
                ]);
            });

            var options = {
                title: 'Lab Testing Results',
                subtitle: 'Successes and Failures',
                width: 700,
                height: 400,
                bars: 'horizontal', // Required for Material Bar Charts.
                //series: {
                //    0: { axis: 'counts' }, // Bind series 0 to an axis named 'distance'.
                //    1: { axis: 'ratio' } // Bind series 1 to an axis named 'brightness'.
                //},
                //axes: {
                //    x: {
                //        counts: { label: 'counts' }, // Bottom x-axis.
                //        ratio: { side: 'top', label: 'ratio' } // Top x-axis.
                //    }
                //}
            };

            var chart = new google.charts.Bar(document.getElementById('chart'));
            chart.draw(chartData, options);
        };
    };

    this.createMachineSummaryChart = function (data, data2) {
        var width = 700,
            height = 600,
            radius = Math.min(width, height) / 2;

        var color = d3.scale.category20c();
        var colors = {
            "Success": "#3182bd",
            "Failure": "#e6550d",
            "Unavailable": "#9e9ac8"
        };

        data.forEach(function (machine) {
            d3.select("#chart").selectAll("*").remove();
            var svg = d3.select("#chart").append("svg")
            .attr("width", width)
            .attr("height", height + (height * 0.05))
            .append("g")
            .attr("transform", "translate(" + width / 2 + "," + height * .52 + ")");

            var partition = d3.layout.partition()
                .sort(null)
                .size([2 * Math.PI, radius * radius])
                .value(function (d) { return 1; });

            var arc = d3.svg.arc()
                .startAngle(function (d) { return d.x; })
                .endAngle(function (d) { return d.x + d.dx; })
                .innerRadius(function (d) { return Math.sqrt(d.y); })
                .outerRadius(function (d) { return Math.sqrt(d.y + d.dy); });

            var slices = svg.datum(machine.root).selectAll("path")
                .data(partition.nodes)
                .enter().append("g");

            var path = slices.append("path")
                .attr("display", function (d) { return d.depth ? null : "none"; }) // hide inner ring
                .attr("d", arc)
                .style("stroke", "#fff")
                .style("fill", getColor)
                .style("fill-rule", "evenodd")
                .append("svg:title")
                .text(function (d) { return d["name"]; });

            slices.append("text")
                .style("font-size", "10px")
                .attr("x", function (d) { return d["children"] ? d["children"].length : 0; })
                .attr("text-anchor", "middle")
                .attr("transform", function (d) { return "translate(" + arc.centroid(d) + ")" + "rotate(" + getAngle(d, arc) + ")"; })
                .attr("dx", "6") // margin
                .attr("dy", ".35em") // vertical-align
                .text(function (d) { return d['name']; })
                .attr("pointer-events", "none");

            d3.selectAll("input").on("change", function change() {
                var value = this.value === "count"
                    ? function () { return 1; }
                    : function (d) { return d.size; };

                path
                    .data(partition.value(value).nodes)
                    .transition()
                    .duration(1500)
                    .attrTween("d", arcTween);
            });
        });

        function getColor(d) {
            if (d.depth == 0)
                d.color = colors["Unavailable"];
            else if (d.depth == 1)
                d.color = colors[d.name];
            else if(d.depth == 2)
                d.color = color(d.parent.name + d.name);
            else if (d.depth == 3) {
                var brightness = (d.name.charCodeAt(0) - 65) * 0.04;
                d.color = d3.rgb(d.parent.color).brighter(brightness);
            } else
                d.color = d3.rgb(d.parent.color).brighter(0);

            return d.color;
        };

        // Stash the old values for transition.
        function stash(d) {
            d.x0 = d.x;
            d.dx0 = d.dx;
        }

        // Interpolate the arcs in data space.
        function arcTween(a) {
            var i = d3.interpolate({ x: a.x0, dx: a.dx0 }, a);
            return function (t) {
                var b = i(t);
                a.x0 = b.x;
                a.dx0 = b.dx;
                return arc(b);
            };
        }

        function getAngle(d, arc) {
            if (d.depth == 0)
                return 0;
            // Offset the angle by 90 deg since the '0' degree axis for arc is Y axis, while
            // for text it is the X axis.
            var thetaDeg = (180 / Math.PI * (arc.startAngle()(d) + arc.endAngle()(d)) / 2 - 90);
            // If we are rotating the text by more than 90 deg, then "flip" it.
            // This is why "text-anchor", "middle" is important, otherwise, this "flip" would
            // a little harder.
            return (thetaDeg > 90) ? thetaDeg - 180 : thetaDeg;
        }
    }
}]);