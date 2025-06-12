import { Container, Typography } from '@mui/material';
import * as React from 'react';

const Footer: React.FC = () => {
    return (
        <footer className='base-block footer text-light'>
            <Container>
                <Typography color='var(--color-mutted)'>Яговкин С.И., 2021—2025</Typography>
            </Container>
        </footer>
    )
}

export default Footer;