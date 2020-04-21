function setStyle(el, styles = {}) {
    for (let style in styles) {
        el.style[style] = styles[style];
    }
}

function isAdjacent(newSelected, allSelected = []) {
    for (let i = 0; i < allSelected.length; i++) {
        if (allSelected[i].seat == (newSelected - 1) || allSelected[i].seat == (newSelected + 1)) {
            return true;
        }
    }

    return allSelected.length > 0 ? false : true;
}

function updateOrderText() {
    let orderedText = '';
    let totalPrice = 0;
    let room = rooms[selectedRoom];

    for (let i = 0; i < selectedSeats.length; i++) {
        orderedText += '\nRow ' + (room.length - selectedSeats[i].row) + ', seat ' + (selectedSeats[i].seat + 1) + ' (' + selectedSeats[i].type + ' ' + selectedSeats[i].price + '$).';
        totalPrice += selectedSeats[i].price;
    }

    let seatDescription = 'Selected ' + (selectedSeats.length) + ' seat' + (selectedSeats.length != 1 ? 's: ' : ': ') + orderedText + '\nTotal price of: $' + (Math.round(totalPrice * 100) / 100);
    document.querySelector('.seat-description').innerText = seatDescription;
}

function loadSeatOverview() {
    let room = rooms[selectedRoom];
    let gridColumn = document.createElement('div');
    let gridRow = document.createElement('div');

    gridColumn.classList.add('grid-column');
    gridRow.classList.add('grid-row');

    for (let column = 0; column < room[0].length; column++) {
        let elSpan = document.createElement('span');
        let txt = document.createTextNode(column + 1);

        elSpan.appendChild(txt);
        elSpan.classList.add('grid-number');
        setStyle(elSpan, { 'width': blockSize + 'px', 'height': blockSize + 'px', left: (blockSize * column + padding / 2 + blockSize) + 'px', 'top': '0px', 'font-size': (15 + blockSize * 3) + '%', 'line-height': blockSize + 'px' });
        gridColumn.appendChild(elSpan);
    }

    for (let row = 0; row < room.length; row++) {
        let elSpan = document.createElement('span');
        let txt = document.createTextNode(room.length - row);

        elSpan.appendChild(txt);
        elSpan.classList.add('grid-number');
        setStyle(elSpan, { 'width': blockSize + 'px', 'height': blockSize + 'px', 'left': '0px', 'top': (blockSize * row + blockSize + padding / 2 + 2) + 'px', 'font-size': (15 + blockSize * 3) + '%', 'line-height': (blockSize / 2) + 'px' });
        gridRow.appendChild(elSpan);
    }

    gridContainer.appendChild(gridColumn);
    gridContainer.appendChild(gridRow);
    setStyle(container, { 'width': (blockSize * room[0].length + padding) + 'px', 'height': (blockSize * room.length + padding) + 'px', 'top': (blockSize / 1.75 + 5) + 'px', 'left': (blockSize) + 'px' });
    setStyle(document.querySelector('.screen-title'), { 'width': (blockSize * room[0].length + padding) + 'px' });
    setStyle(document.querySelector('.controls'), { 'margin-left': (blockSize + 15) + 'px' });

    for (let row = 0; row < room.length; row++) {
        for (let seat = 0; seat < room[row].length; seat++) {
            let seatType = room[row][seat];

            if (seatType != 0) {
                let elSeat = document.createElement('div');
                elSeat.classList.add('seat', 'row-' + row, 'seat-' + seat, seatTypes[seatType - 1]);
                setStyle(elSeat, {
                    'top': (row * blockSize + (padding / 2)) + 'px',
                    'left': (seat * blockSize + (padding / 2)) + 'px',
                    'width': blockSize + 'px',
                    'height': blockSize + 'px'
                });

                for (let i = 0; i < selectedSeats.length; i++) {
                    if (selectedSeats[i].row == row && selectedSeats[i].seat == seat) {
                        elSeat.classList.add('selected');
                    }
                }
                
                container.appendChild(elSeat);

                elSeat.addEventListener('click', (event, selectedRow = row, selectedSeat = seat, type = seatType) => {
                    let price = seatPrices[type - 1];
                    let selected = document.querySelector('.seat-' + selectedSeat + '.row-' + selectedRow);

                    if (selected.classList.contains('selected')) {
                        selected.classList.remove('selected');

                        for (let i = 0; i < selectedSeats.length; i++) {
                            if (selectedSeats[i].row == selectedRow && selectedSeats[i].seat == selectedSeat) {
                                selectedSeats.splice(i, 1);
                            }
                        }
                    }
                    else {
                        let errorMsg = document.querySelector('.error-msg');

                        if (!selectedSeats.length || selectedSeats[0].row == selectedRow) {
                            if (isAdjacent(selectedSeat, selectedSeats)) {
                                selected.classList.add('selected');
                                selectedSeats.push({ 'row': selectedRow, 'seat': selectedSeat, 'type': seatTypes[type - 1], 'price': price });
                            }
                            else {
                                errorMsg.innerText = 'Je kan alleen stoelen naast je geselecteerde stoelen selecteren!';
                            }
                        }
                        else {
                            errorMsg.innerText = 'Je kan alleen stoelen van dezelfde rij selecteren!';
                        }
                    }

                    updateOrderText();
                });
            }
        }
    }
}

document.querySelector('.controls button.zoom-in').addEventListener('click', () => {
    blockSize < 30 ? blockSize += 2 : 30;
    gridContainer.innerHTML = '<div id="cinema-room"></div>';
    container = document.querySelector('#cinema-room');
    loadSeatOverview();
});
document.querySelector('.controls button.zoom-out').addEventListener('click', () => {
    blockSize > 6 ? blockSize -= 2 : 6;
    gridContainer.innerHTML = '<div id="cinema-room"></div>';
    container = document.querySelector('#cinema-room');
    loadSeatOverview();
});

// Settings for prototype/demo
let rooms = [
    // Screen auditorium 1
    [
        [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
        [1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1],
        [1, 1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 1],
        [1, 1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 1],
        [1, 1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 1],
        [1, 1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 1],
        [1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1],
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
        [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0]
    ],

    // Screen auditorium 2
    [
        [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0],
        [0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0],
        [0, 1, 1, 1, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2, 1, 1, 1, 0],

        [1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1],
        [1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 1, 1, 1],
        [1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1],
        [1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1],
        [1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1],

        [0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 0],
        [0, 1, 1, 1, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2, 1, 1, 1, 0],
        [0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0],
        [0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0],
        [0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0],

        [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0]
    ],

    // Screen auditorium 3
    [
        [0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0],

        [0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 0],
        [1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1],

        [1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1],
        [0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0],
        [0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0],
        [0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0],
    ]
];
let gridContainer = document.querySelector('#cinema-grid');
let container = document.querySelector('#cinema-room');
let selectedRoom = 2;
let blockSize = 20;
let padding = 20;
let seatPrices = [7.99, 12.99, 17.99];
let seatTypes = ['normal', 'luxery', 'vip'];
let selectedSeats = [];

loadSeatOverview();