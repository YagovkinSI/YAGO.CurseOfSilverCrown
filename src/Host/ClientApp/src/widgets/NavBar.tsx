import * as React from 'react';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { useNavigate } from 'react-router-dom';
import { useMediaQuery, useTheme } from '@mui/material';
import LoginIconMenu from '../features/LoginIconMenu';

const NavBar: React.FC = () => {
    const theme = useTheme();
    const isSm = useMediaQuery(theme.breakpoints.up('sm'));
    const navigate = useNavigate()

    const onLinkClick = (path: string) => {
        navigate(path)
    }

    const renderLogo = () => {
        return (
            <>
                <Typography
                    variant={isSm ? 'h5' : 'h6'}
                    noWrap
                    onClick={() => onLinkClick('/')}
                    sx={{
                        mr: 2,
                        display: 'flex',
                        flexGrow: { xs: 1, sm: 0 },
                        justifyContent: { xs: 'center', sm: 'start' },
                        fontWeight: 700,
                        letterSpacing: '.3rem',
                        color: 'inherit',
                        textDecoration: 'none',
                        cursor: 'pointer'
                    }}
                >
                    YAGO World
                </Typography>
            </>
        )
    }

    return (
        <Toolbar disableGutters sx={{justifyContent: 'space-between'}}>
            {renderLogo()}
            <LoginIconMenu />
        </Toolbar>
    );
}

export default NavBar;