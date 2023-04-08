import * as React from 'react';
import StyleConstants from '../StyleConstants';
import { Container } from 'react-bootstrap';

const style = {
    bottom: `0px`,
    height: `${StyleConstants.heightFooter}`,
    width: '100%',
    color: `${StyleConstants.colorUltraLight}`,
    backgroundColor: `${StyleConstants.colorDark}`,
    borderTop: `2px solid ${StyleConstants.colorBright}`,
    zIndex: `${StyleConstants.zIndexFooter}`
};

const TopPanel: React.FC = () => {
    return (
        <footer className='positionAbsolute' style={style}>
            <Container>
                <p style={{ textAlign: 'center' }}>&copy; Яговкин С.И., 2021—2023</p>
            </Container>
        </footer>
    );
}

export default TopPanel;
