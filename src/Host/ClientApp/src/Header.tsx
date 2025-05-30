import { Container, Typography } from '@mui/material';
import * as React from 'react';

const Header: React.FC = () => {
    return (
        <header className='base-block header text-light'>
            <Container >
                <Typography variant='h5'>
                    YAGO Fantasy World
                </Typography>
            </Container>
        </header>
    );
}

export default Header;