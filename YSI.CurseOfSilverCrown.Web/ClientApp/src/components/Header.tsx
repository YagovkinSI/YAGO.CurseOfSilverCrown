import * as React from 'react';
import StyleConstants from '../StyleConstants';

const style = {
    height: `${StyleConstants.heightHeader}`,
    width: '100%',
    color: `${StyleConstants.colorUltraLight}`,
    backgroundColor: `${StyleConstants.colorDark}`,
    borderBottom: `2px solid ${StyleConstants.colorBright}`,
    zIndex: 100
};

const Header: React.FC = () => {
    return (
        <header className='positionAbsolute' style={style}>
            <h1 style={{ textAlign: 'center', fontSize: '1.4rem' }}>
                <a href='https://almusahan.ru/Home'>Проклятие Серебряной Короны</a>
            </h1>
            <p style={{ textAlign: 'center' }}>
                В будущем тут будет важная игровая информация.
                А пока игра проходит по <a href='https://almusahan.ru/Home'>ссылке</a>
            </p>
        </header>
    );
}

export default Header;
