import * as React from 'react';
import { useAppSelector } from '../store';
import { NavItem, NavLink } from 'reactstrap';
import NavMenuItem from './NavMenuItem';
import { Spinner } from 'react-bootstrap';


const LoginMenu: React.FC = () => {
    const state = useAppSelector(state => state.userReducer);

    const menuForUser = () => {
        return (
            <ul className="navbar-nav">
                <NavItem>
                    <NavLink className="text-dark">Здравствуйте, {state.user?.userName}!</NavLink>
                </NavItem>
                <NavMenuItem name='Выйти' path='/Logout' />
            </ul>
        );
    }

    const menuForGuest = () => {
        return (
            <ul className="navbar-nav">
                <NavMenuItem name='Регистрация' path='/Register' />
                <NavMenuItem name='Вход' path='/Login' />
            </ul>
        );
    }

    const loadingMenu = () => {
        return (
            <React.Fragment>
                <Spinner animation="border" />
                'Загрузка...'
            </React.Fragment>
        );
    }

    return state.isLoading
        ? loadingMenu()
        : state.user != undefined
            ? menuForUser()
            : menuForGuest();
}

export default LoginMenu;
