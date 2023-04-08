import * as React from 'react';
import StyleConstants from '../StyleConstants';

const modalHeight = 300;
const modalWidth = 350;
const margin = 5;

const divStyle = {
  top: `calc(100% - ${modalHeight + margin}px - ${StyleConstants.heightFooter})`,
  left: `calc(50% - ${modalWidth / 2}px)`,
  height: `${modalHeight}px`,
  width: `${modalWidth}px`,
  color: `${StyleConstants.colorUltraDark}`,
  backgroundColor: `${StyleConstants.colorLight}`,
  border: `2px solid ${StyleConstants.colorBright}`,
  zIndex: `${StyleConstants.zIndexMain}`
};

const Main: React.FC = () => {
  return (
    <main className='positionAbsolute' style={divStyle}>
      <h2 style={{ textAlign: 'center' }}>
        В будущем тут будет важная игровая информация.
        А пока игра проходит по <a href='https://almusahan.ru/Home'>ссылке</a>
      </h2>
    </main>
  );
}

export default Main;
