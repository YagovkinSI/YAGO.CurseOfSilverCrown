import * as React from 'react';
import StyleConstants from '../StyleConstants';
import GameMap from './GameMap';

const style = {
  top: `${StyleConstants.heightHeader}`,
  height: `calc(100% - ${StyleConstants.heightHeader} - ${StyleConstants.heightFooter})`,
  width: '100%',
  color: `${StyleConstants.colorUltraLight}`,
  backgroundColor: `${StyleConstants.colorSemiDark}`,
  zIndex: `${StyleConstants.zIndexNav}`
};

const Nav: React.FC = () => {
  return (
    <nav className='positionAbsolute' style={style}>
      <GameMap />
    </nav>
  );
}

export default Nav;
