import * as React from 'react';
import { Avatar, Box, IconButton, Menu, MenuItem, Tooltip, Typography } from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import { useNavigate } from 'react-router-dom';
import YagoAvatar from '../shared/YagoAvatar';
import type YagoLink from '../entities/YagoLink';
import { useGetAuthorizationDataQuery } from '../entities/AuthorizationData';

const userTemporaryProfileLinks: YagoLink[] = [
    { name: 'Изменить', path: '/registration' },
    { name: 'Выход', path: '/logout' },
];

const userProfileLinks: YagoLink[] = [
    { name: 'Выход', path: '/logout' },
];

const guestProfileLinks: YagoLink[] = [
    { name: 'Авторизация', path: '/registration' },
];

const LoginIconMenu: React.FC = () => {
    const { data } = useGetAuthorizationDataQuery()
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
            <Tooltip title="Меню управления аккаунтом">
                <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                    {data?.isAuthorized && data.user != undefined
                        ? <YagoAvatar name={data.user.userName} />
                        :
                        <Avatar
                            sx={{
                                height: { xs: '30px', sm: '40px' },
                                width: { xs: '30px', sm: '40px' }
                            }} >
                            <PersonIcon />
                        </Avatar>
                    }
                </IconButton>
            </Tooltip>
        )
    }

    const renderLoginMenuLinks = () => {
        const userMenuLinks = data?.isAuthorized
            ? data.user!.isTemporary
                ? userTemporaryProfileLinks
                : userProfileLinks
            : guestProfileLinks;
        return userMenuLinks.map((link) => (
            <MenuItem key={link.name} onClick={() => { onLinkClick(link.path!); handleCloseUserMenu() }}>
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