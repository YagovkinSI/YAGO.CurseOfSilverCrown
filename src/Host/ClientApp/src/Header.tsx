import { Container } from '@mui/material';
import * as React from 'react';
import NavBar from './widgets/NavBar';

const Header: React.FC = () => {
    return (
        <header className='base-block header text-light'>
            <Container >
                <NavBar />
            </Container>
        </header>
    );
}

export default Header;