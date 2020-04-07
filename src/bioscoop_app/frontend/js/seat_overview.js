function setStyle(el, styles = {}) {
    for (let style in styles) {
        el.style[style] = styles[style];
    }
}

function loadSeatOverview() {
    let room = rooms[selectedRoom];
    let seatTypes = ['normal', 'luxery', 'vip'];

    for (let row = 0; row < room.length; row++) {
        for (let seat = 0; seat < room[row].length; seat++) {
            let seatType = room[row][seat];

            if (seatType != 0) {
                let elSeat = document.createElement('div');
                elSeat.classList.add('seat', 'seat-' + seat, seatTypes[seatType - 1]);
                setStyle(elSeat, {
                    'top': (row * blockSize) + 'px',
                    'left': (seat * blockSize) + 'px',
                    'width': blockSize + 'px',
                    'height': blockSize + 'px'
                });
                setStyle(container, { 'width': (blockSize * room[0].length + 2) + 'px', 'height': (blockSize * room.length + 2) + 'px' });
                
                container.appendChild(elSeat);
                elSeat.addEventListener('click', (event, selectedRow = row, selectedSeat = seat, type = seatType) => {
                    let price = seatPrices[type - 1];
                    document.querySelector('.seat-description').innerText = 'Selected seat ' + selectedSeat + ' ( ' + seatTypes[type - 1] + ' ) on row ' + selectedRow + ' ( ' + price + '$ ).';
                });
            }
        }
    }
}

document.querySelector('.controls button.zoom-in').addEventListener('click', () => { blockSize < 30 ? blockSize += 2 : 30; container.innerHTML = ''; loadSeatOverview() });
document.querySelector('.controls button.zoom-out').addEventListener('click', () => { blockSize > 6 ? blockSize -= 2 : 6; container.innerHTML = ''; loadSeatOverview() });

// Settings for prototype/demo
let rooms = [[
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
    [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0]]
];
let container = document.querySelector('#cinema-room');
let selectedRoom = 0;
let blockSize = 20;
let seatPrices = [7.99, 12.99, 17.99];

loadSeatOverview();