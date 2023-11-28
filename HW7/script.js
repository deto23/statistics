
const myCanvas = document.getElementById("myChart");
const myCanvas2 = document.getElementById("myChart2");

const ctx = myCanvas.getContext("2d");
const ctx2 = myCanvas2.getContext("2d");

function hideAllDivs() {
    let allDivs = document.querySelectorAll('div');
    allDivs.forEach(function (div) {
      div.style.display = 'none';
    });
}

const div_selector = document.getElementById("equation-selector");
const div_chart = document.getElementById('chart_div');
const div_chart2 = document.getElementById('chart2_div');

const ab_div = document.getElementById('ab_div');
const gb_div = document.getElementById('gb_div');
const ou_div = document.getElementById('ou_div');
const v_div = document.getElementById('v_div');
const hw_div = document.getElementById('hw_div');
const cir_div = document.getElementById('cir_div');
const bk_div = document.getElementById('bk_div');
const h_div = document.getElementById('h_div');
const cm_div = document.getElementById('cm_div');


document.getElementById('equationSelect').addEventListener('change', function () {
    let selectedValue = this.value;

    // Esempi di azioni in base alla selezione
    switch (selectedValue) {
        case 'null':
            hideAllDivs();
            div_selector.style.display = 'block';
            break;
        case 'ab':
            hideAllDivs();
            div_selector.style.display = 'block';
            ab_div.style.display = 'block';
            break;
        case 'gb':
            hideAllDivs();
            div_selector.style.display = 'block';
            gb_div.style.display = 'block';
            break;
        case 'ou':
            hideAllDivs();
            div_selector.style.display = 'block';
            ou_div.style.display = 'block';
            break;
        case 'v':
            hideAllDivs();
            div_selector.style.display = 'block';
            v_div.style.display = 'block';
            break;
        case 'hw':
            hideAllDivs();
            div_selector.style.display = 'block';
            hw_div.style.display = 'block';
            break;
        case 'cir':
            hideAllDivs();
            div_selector.style.display = 'block';
            cir_div.style.display = 'block';
            break;
        case 'bk':
            hideAllDivs();
            div_selector.style.display = 'block';
            bk_div.style.display = 'block';
            break;
        case 'h':
            hideAllDivs();
            div_selector.style.display = 'block';
            h_div.style.display = 'block';
            break;
        case 'cm':
            hideAllDivs();
            div_selector.style.display = 'block';
            cm_div.style.display = 'block';
            break;
    }
});


function arithmetic_brownian() {

    const M = document.getElementById("ab_m").value;
    const N = document.getElementById("ab_n").value;
    const mu = document.getElementById("ab_mu").value;
    const sigma = document.getElementById("ab_sigma").value;
    const dt = document.getElementById("ab_dt").value;

    const allData = [];

    for (let i = 0; i < M; i++) {
        let currentValue = 0;
        const yValues = [0];

        for (let j = 0; j < N; j++) {

            const dW = sigma * Math.sqrt(dt) * (2 * Math.random() - 1);

            currentValue = currentValue + mu * dt + dW;

            yValues.push(currentValue);

        }

        allData.push(yValues);
    }
    

    drawChart(allData, M);

}

function geometric_brownian() {
    const M = document.getElementById("gb_m").value;
    const N = document.getElementById("gb_n").value;
    const mu = document.getElementById("gb_mu").value;
    const sigma = document.getElementById("gb_sigma").value;
    const dt = document.getElementById("gb_dt").value;

    const allData = [];

    for (let i = 0; i < M; i++) {
        const initialValue = 100;
        let currentValue = initialValue;
        const yValues = [initialValue];

        for (let j = 0; j < N; j++) {

            const dW = sigma * Math.sqrt(dt) * (2 * Math.random() - 1);

            currentValue = currentValue * Math.exp((mu - 0.5 * sigma ** 2) * dt + sigma * dW);

            yValues.push(currentValue);

        }

        allData.push(yValues);
    }
    
    drawChart(allData, M);

}

function ornstein_uhlenbeck(){
    const M = document.getElementById("ou_m").value;
    const N = document.getElementById("ou_n").value;
    const sigma = document.getElementById("ou_sigma").value * 2;
    const theta = document.getElementById("ou_theta").value / 2;
    const dt = document.getElementById("ou_dt").value;

    const allData = [];

    for (let i = 0; i < M; i++) {
        let initialValue = Math.random() * 100;
        let currentValue = initialValue;
        const yValues = [currentValue];

        for (let j = 0; j < N; j++) {

            const dW = sigma * Math.sqrt(dt) * (2 * Math.random() - 1);

            const drift = theta * (initialValue - currentValue) * dt;
            currentValue = currentValue + drift + dW;

            initialValue = currentValue;

            yValues.push(currentValue);

        }

        allData.push(yValues);
    }
    
    drawChart(allData, M);

}

function vasicek(){
    const M = document.getElementById("v_m").value;
    const N = document.getElementById("v_n").value;
    const sigma = document.getElementById("v_sigma").value;
    const theta = document.getElementById("v_theta").value;
    const kappa = document.getElementById("v_kappa").value;
    const dt = document.getElementById("v_dt").value;

    const allData = [];

    for (let i = 0; i < M; i++) {
        let initialValue = 0.03;
        let currentValue = initialValue;
        const yValues = [currentValue];

        for (let j = 0; j < N; j++) {

            const dW = sigma * Math.sqrt(dt) * (2 * Math.random() - 1);

            const drift = kappa * (theta - currentValue) * dt;
            currentValue = currentValue + drift + dW;

            yValues.push(currentValue);

        }

        allData.push(yValues);
    }
    
    drawChart(allData, M);

}

function hull_white(){
    const M = document.getElementById("hw_m").value;
    const N = document.getElementById("hw_n").value;
    const sigma = document.getElementById("hw_sigma").value;
    const theta = document.getElementById("hw_theta").value;
    const a = document.getElementById("hw_a").value;
    const dt = document.getElementById("hw_dt").value;

    const allData = [];

    for (let i = 0; i < M; i++) {
        let initialValue = 0.025;
        let currentValue = initialValue;
        const yValues = [currentValue];

        for (let j = 0; j < N; j++) {
            const randomVolatility = sigma * Math.random();
            const randomMean = theta * Math.random();

            const dW = sigma * Math.sqrt(dt) * (2 * Math.random() - 1);

            const drift = (randomMean - a * currentValue) * dt;
            currentValue = currentValue + drift + dW;

            yValues.push(currentValue);

        }

        allData.push(yValues);
    }
    
    drawChart(allData, M);
}

function cox_ingersoll_ross(){
    const M = document.getElementById("cir_m").value;
    const N = document.getElementById("cir_n").value;
    const sigma = document.getElementById("cir_sigma").value;
    const theta = document.getElementById("cir_theta").value;
    const kappa = document.getElementById("cir_kappa").value;
    const dt = document.getElementById("cir_dt").value;

    const allData = [];

    for (let i = 0; i < M; i++) {
        let initialValue = 0.03;
        let currentValue = initialValue;
        const yValues = [currentValue];

        for (let j = 0; j < N; j++) {

            const dW = Math.sqrt(dt) * (2 * Math.random() - 1);

            const drift = kappa * (theta - currentValue) * dt;
            const diffusion = sigma * Math.sqrt(currentValue) * dW;
            currentValue = Math.max(0, currentValue + drift + diffusion);

            yValues.push(currentValue);

        }

        allData.push(yValues);
    }
    
    drawChart(allData, M);
}

function black_karasinski(){
    const M = document.getElementById("bk_m").value;
    const N = document.getElementById("bk_n").value;
    const sigma = document.getElementById("bk_sigma").value;
    const theta = document.getElementById("bk_theta").value;
    const a = document.getElementById("bk_a").value;
    const dt = document.getElementById("bk_dt").value;

    const allData = [];

    for (let i = 0; i < M; i++) {
        let initialValue = 0.025;
        let currentValue = initialValue;
        const yValues = [currentValue];

        for (let j = 0; j < N; j++) {
            const randomVolatility = sigma * Math.random();
            const randomMean = theta * Math.random();

            const dW = Math.sqrt(dt) * (2 * Math.random() - 1);

            const drift = (randomMean - a * currentValue) * dt;
            const diffusion = randomVolatility * Math.sqrt(currentValue) * dW;
            currentValue = Math.max(0, currentValue + drift + diffusion);

            yValues.push(currentValue);

        }

        allData.push(yValues);
    }
    
    drawChart(allData, M);
}

function drawChart(attacks, systems) {  
    div_chart.style.display = 'block';

    const existingChart = Chart.getChart("myChart");
    if (existingChart) {
        existingChart.destroy();
    }

    ctx.clearRect(0, 0, myCanvas.width, myCanvas.height);

    const datasets = [];
    const colorSystems = [];

    for (let i = 0; i < systems; i++) {
        const randomColor = getRandomRGBAColor();
        colorSystems.push(randomColor);
  
        datasets.push({
            label: `System ${i + 1}`,
            fill: false,
            backgroundColor: randomColor,
            borderColor: randomColor,
            data: attacks[i],
        });
    }

    const newChart = new Chart(myCanvas, {
        type: "line",
        data: {
            labels: Array.from({ length: attacks[0].length }, (_, i) => i),
            datasets: datasets,
        },
        options: {
            plugins: {
                title: {
                    display: true,
                    text: 'Simulation',
                    font: {
                        size: 18
                    }
                },
                legend: {
                    display: false
                },
            }
        }
    });
}

function heston(){
    const M = document.getElementById("h_m").value;
    const N = document.getElementById("h_n").value;
    const sigma = document.getElementById("h_sigma").value;
    const theta = document.getElementById("h_theta").value;
    const mu = document.getElementById("h_mu").value;
    const kappa = document.getElementById("h_kappa").value;
    const rho = document.getElementById("h_rho").value;
    const dt = document.getElementById("h_dt").value;

    const allData = [];
    const initialStockPrice = 100;
    const initialVolatility = 0.2;

    for (let i = 0; i < M; i++) {

        let currentStockPrice = initialStockPrice;
        let currentVolatility = initialVolatility;

        const yValuesStockPrice = [currentStockPrice];
        const yValuesVolatility = [initialVolatility];

        for (let j = 0; j < N; j++) {

        const dW1 = Math.sqrt(dt) * (2 * Math.random() - 1);
        const dW2 = rho * dW1 + Math.sqrt(1 - rho**2) * Math.sqrt(dt) * (2 * Math.random() - 1);

        const dS = mu * currentStockPrice * dt + Math.sqrt(currentVolatility) * currentStockPrice * dW1;
        const dv = kappa * (theta - currentVolatility) * dt + sigma * Math.sqrt(currentVolatility) * dW2;

        currentStockPrice = currentStockPrice + dS;
        currentVolatility = Math.max(0, currentVolatility + dv);

        yValuesStockPrice.push(currentStockPrice);
        yValuesVolatility.push(currentVolatility);

        }

        allData.push([yValuesStockPrice, yValuesVolatility]);
    }
    
    drawChart2(allData, M);

}

function chen_model(){
    const M = document.getElementById("cm_m").value;
    const N = document.getElementById("cm_n").value;
    const sigma = document.getElementById("cm_sigma").value;
    const theta = document.getElementById("cm_theta").value;
    const mu = document.getElementById("cm_mu").value;
    const kappa = document.getElementById("cm_kappa").value;
    const beta = document.getElementById("cm_beta").value;
    const dt = document.getElementById("cm_dt").value;

    const allData = [];
    const initialStockPrice = 100;
    const initialVolatility = 0.2;

    for (let i = 0; i < M; i++) {

        let currentStockPrice = initialStockPrice;
        let currentVolatility = initialVolatility;

        const yValuesStockPrice = [currentStockPrice];
        const yValuesVolatility = [initialVolatility];

        for (let j = 0; j < N; j++) {

        const dW1 = Math.sqrt(dt) * (2 * Math.random() - 1);
        const dW2 = Math.sqrt(dt) * (2 * Math.random() - 1);

        const dS = mu * currentStockPrice * dt + Math.sqrt(currentVolatility) * currentStockPrice * dW1;
        const dv = kappa * (theta - currentVolatility) * dt + sigma * currentVolatility**beta * dW2;

        currentStockPrice = currentStockPrice + dS;
        currentVolatility = Math.max(0, currentVolatility + dv);

        yValuesStockPrice.push(currentStockPrice);
        yValuesVolatility.push(currentVolatility);

        }

        allData.push([yValuesStockPrice, yValuesVolatility]);
    }
    
    drawChart2(allData, M);

}

function drawChart2(attacks, systems) {  
    div_chart.style.display = 'block';
    div_chart2.style.display = 'block';

    const existingChart = Chart.getChart("myChart");
    const existingChart2 = Chart.getChart("myChart2");
    if (existingChart) {
        existingChart.destroy();
    }
    if (existingChart2) {
        existingChart2.destroy();
    }

    ctx.clearRect(0, 0, myCanvas.width, myCanvas.height);
    ctx2.clearRect(0, 0, myCanvas2.width, myCanvas2.height);

    const datasets = [];
    const datasets2 = [];
    const colorSystems = [];

    for (let i = 0; i < systems; i++) {
        const randomColor = getRandomRGBAColor();
        colorSystems.push(randomColor);

        datasets.push({
            label: `System ${i + 1} - Stock Prices`,
            fill: false,
            backgroundColor: randomColor,
            borderColor: randomColor,
            data: attacks[i][0], // Accedi all'array dei prezzi del sottostante
        });
        datasets2.push({
            label: `System ${i + 1} - Volatilities`,
            fill: false,
            backgroundColor: randomColor,
            borderColor: randomColor,
            data: attacks[i][1], // Accedi all'array della volatilità
        });
    }

    const newChart = new Chart(myCanvas, {
        type: "line",
        data: {
            labels: Array.from({ length: attacks[0][0].length }, (_, i) => i),
            datasets: datasets,
        },
        options: {
            plugins: {
                title: {
                    display: true,
                    text: 'Stock Prices Simulation',
                    font: {
                        size: 18
                    }
                },
                legend: {
                    display: false
                },
            }
        }
    });
    const newChart2 = new Chart(myCanvas2, {
        type: "line",
        data: {
            labels: Array.from({ length: attacks[0][1].length }, (_, i) => i),
            datasets: datasets2,
        },
        options: {
            plugins: {
                title: {
                    display: true,
                    text: 'Volatilities Simulation',
                    font: {
                        size: 18
                    }
                },
                legend: {
                    display: false
                },
            }
        }
    });
}



function getRandomRGBAColor() {
    const r = Math.floor(Math.random() * 256);
    const g = Math.floor(Math.random() * 256);
    const b = Math.floor(Math.random() * 256);
    const a = Math.random();
  
    return `rgba(${r}, ${g}, ${b}, ${a})`;
}
  



//---------------------------------------------------------------------------------------------------------
interact('#chart_div')
.resizable({
  edges: { bottom: true, right: true },
  listeners: {
    move(event) {
      const target = event.target;
      const x = (parseFloat(target.getAttribute('data-x')) || 0);
      const y = (parseFloat(target.getAttribute('data-y')) || 0);

      target.style.width = event.rect.width + 'px';
      target.style.height = event.rect.height + 'px';

      target.style.webkitTransform = target.style.transform = 'translate(' + x + 'px,' + y + 'px)';
    }
  }
})
.draggable({
  modifiers: [
    interact.modifiers.restrictRect({
      //restriction: 'parent', // Imposta la restrizione al genitore (il contenitore del grafico)
      endOnly: true,
    }),
  ],
  listeners: {
    move(event) {
      const target = event.target;
      const x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx;
      const y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

      target.style.webkitTransform = target.style.transform = 'translate(' + x + 'px,' + y + 'px)';

      target.setAttribute('data-x', x);
      target.setAttribute('data-y', y);
    }
  }
});

interact('#chart2_div')
.resizable({
  edges: { bottom: true, right: true },
  listeners: {
    move(event) {
      const target = event.target;
      const x = (parseFloat(target.getAttribute('data-x')) || 0);
      const y = (parseFloat(target.getAttribute('data-y')) || 0);

      target.style.width = event.rect.width + 'px';
      target.style.height = event.rect.height + 'px';

      target.style.webkitTransform = target.style.transform = 'translate(' + x + 'px,' + y + 'px)';
    }
  }
})
.draggable({
  modifiers: [
    interact.modifiers.restrictRect({
      //restriction: 'parent', // Imposta la restrizione al genitore (il contenitore del grafico)
      endOnly: true,
    }),
  ],
  listeners: {
    move(event) {
      const target = event.target;
      const x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx;
      const y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

      target.style.webkitTransform = target.style.transform = 'translate(' + x + 'px,' + y + 'px)';

      target.setAttribute('data-x', x);
      target.setAttribute('data-y', y);
    }
  }
});

//---------------------------------------------------------------------------------------------------------