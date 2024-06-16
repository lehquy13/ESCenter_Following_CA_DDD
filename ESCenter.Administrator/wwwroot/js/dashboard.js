function ChartReload(url) {

    $.ajax({
        type: "GET",
        url: url,
        data: {},
        success: function (res) {
            if (res.res === true) {
                $("#lineChartCard .card-body").html(res.partialView);
            }
        }
    })
}

function AreaChartReload(url) {
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        success: function (res) {
            if (res.res === true) {
                $("#areaChartCard .card-body").html(res.partialView);
            }

        }
    })
}

function PieChartReload(url) {

    $.ajax({
        type: "GET",
        url: url,
        data: {},
        success: function (res) {
            if (res.res === true) {
                $("#pieChartCard .card-body").html(res.partialView);
            }
            // var obj1 = JSON.parse(res.pieWeekData1);
            // var obj2 = JSON.parse(res.pieWeekData2);
            //
            // console.log(obj1);
            // console.log(obj2);
            //
            // const chart = window.pieChartStore;
            //
            // chart.updateOptions({
            //     series: obj1,
            //     labels: obj2
            // })

        }
    })
}

function TotalValueCardReload(url) {
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        success: function (res) {
            if (res.res === true) {
                $("#" + res.viewName + " .card-body").html(res.partialView);
            }

        }
    })
}