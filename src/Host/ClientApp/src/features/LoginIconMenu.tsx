import * as React from 'react';
import { Avatar, Box, IconButton, Menu, MenuItem, Tooltip, Typography } from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import { useNavigate } from 'react-router-dom';
import type YagoLink from '../entities/YagoLink';
import { useGetCurrentUserQuery } from '../entities/CurrentUser';
import YagoAvatar from '../shared/YagoAvatar';

const userProfileLinks: YagoLink[] = [
    { name: 'Выход', path: '/Identity/Account/Logout' },
];

const guestProfileLinks: YagoLink[] = [
    { name: 'Вход', path: '/Identity/Account/Login' },
    { name: 'Регистрация', path: '/Identity/Account/Register' },
];

const LoginIconMenu: React.FC = () => {
    const { data } = useGetCurrentUserQuery()
    const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);
    const navigate = useNavigate()

    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };

    const onLinkClick = (path: string) => {
        navigate(path)
    }

    const renderLoginMenuTooltip = () => {
        return (
            <Tooltip title="Open settings">
                <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                    {data?.isAuthorized && data.user != undefined
                        ? <YagoAvatar name={data.user.userName} />
                        :
                        <Avatar>
                            <PersonIcon />
                        </Avatar>
                    }
                </IconButton>
            </Tooltip>
        )
    }

    const renderLoginMenuLinks = () => {
        const userMenuLinks = data?.isAuthorized
            ? userProfileLinks
            : guestProfileLinks;
        return userMenuLinks.map((link) => (
            <MenuItem key={link.name} onClick={() => { onLinkClick(link.path); handleCloseUserMenu() }}>
                <Typography textAlign="center">{link.name}</Typography>
            </MenuItem>
        ))
    }

    const render = () => {
        return (
            <Box sx={{ flexGrow: 0 }}>
                {renderLoginMenuTooltip()}
                <Menu
                    sx={{ mt: '45px' }}
                    id="menu-appbar"
                    anchorEl={anchorElUser}
                    anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
                    keepMounted
                    transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                    open={Boolean(anchorElUser)}
                    onClose={handleCloseUserMenu}
                >
                    {renderLoginMenuLinks()}
                </Menu>
            </Box>
        )
    }

    return render();
}

export default LoginIconMenu;