import * as React from 'react';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { useNavigate } from 'react-router-dom';
import { useMediaQuery, useTheme } from '@mui/material';
import LoginIconMenu from '../features/LoginIconMenu';
import type YagoLink from '../entities/YagoLink';

const links: YagoLink[] =
    [
        { name: 'Главная', path: '/' },
        { name: 'Владение', path: '/Domain' },
        { name: 'Карта', path: '/app/map' },
        { name: 'История', path: '/History' },
        { name: 'Список фракций', path: '/app/factions' }
    ];

const NavBar: React.FC = () => {
    const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);
    const theme = useTheme();
    const isSm = useMediaQuery(theme.breakpoints.up('sm'));

    const renderMenuIcon = () => {
        return (
            <Box sx={{ display: { xs: 'flex', sm: 'none' } }}>
                <IconButton
                    size="large"
                    aria-label="main menu"
                    aria-controls="menu-appbar"
                    aria-haspopup="true"
                    onClick={(event) => setAnchorElNav(event.currentTarget)}
                    color="inherit"
                >
                    <MenuIcon />
                </IconButton>
                {renderMenu()}
            </Box>
        )
    }

    const renderMenu = () => {
        return (
            <Menu
                id="menu-appbar"
                anchorEl={anchorElNav}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'left', }}
                keepMounted
                transformOrigin={{ vertical: 'top', horizontal: 'left', }}
                open={Boolean(anchorElNav)}
                onClose={() => setAnchorElNav(null)}
                sx={{
                    display: { xs: 'block', sm: 'none' },
                }}
            >
                {links.map((link: YagoLink) => (
                    <MenuItem key={link.path} onClick={() => onLinkClick(link.path!)}>
                        <Typography textAlign="center">{link.name}</Typography>
                    </MenuItem>
                ))}
            </Menu>
        )
    }

    const navigate = useNavigate()
    const onLinkClick = (path: string) => {
        navigate(path)
        setAnchorElNav(null)
    }

    const renderLogo = () => {
        return (
            <>
                <Typography
                    variant={isSm ? 'h6' : 'h5'}
                    noWrap
                    onClick={() => onLinkClick('/')}
                    sx={{
                        mr: 2,
                        display: 'flex',
                        flexGrow: { xs: 1, sm: 0 },
                        justifyContent: { xs: 'center', sm: 'start' },
                        fontFamily: 'monospace',
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

    const renderLinks = () => {
        return <Box sx={{ flexGrow: 1, display: { xs: 'none', sm: 'flex' } }}>
            {links.map((link) => (
                <Button
                    key={link.path}
                    onClick={() => onLinkClick(link.path!)}
                    sx={{ my: 2, color: 'white', display: 'block' }}
                >
                    {link.name}
                </Button>
            ))}
        </Box>
    }

    return (
        <Toolbar disableGutters>
            {renderMenuIcon()}
            {renderLogo()}
            {renderLinks()}
            <LoginIconMenu />
        </Toolbar>
    );
}
export default NavBar;