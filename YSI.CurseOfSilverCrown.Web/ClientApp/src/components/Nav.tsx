import * as React from 'react';
import StyleConstants from '../StyleConstants';

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
      <h3 style={{ textAlign: 'center' }}>Здесь будет интерактивная игровая карта</h3>
    </nav>
  );
}

export default Nav;
