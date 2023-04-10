import * as React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { AppDispatch } from '..';
import { ApplicationState } from '../store';
import { NavItem, NavLink } from 'reactstrap';
import NavMenuItem from './NavMenuItem';


const LoginMenu: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const appState = useSelector(state => state as ApplicationState);

    const state = appState.user;

    if (state != undefined && state.isSignedIn) {
        return (
            <ul className="navbar-nav">
                <NavItem>
                    <NavLink className="text-dark">Здравствуйте, {state.userName}!</NavLink>
                </NavItem>
                <NavMenuItem name='Выйти' path='/Leave' />
            </ul>
        );
    }
    else {
        return (
            <ul className="navbar-nav">
                <NavMenuItem name='Регистрация' path='/Register' />
                <NavMenuItem name='Вход' path='/Login' />
            </ul>
        );
    }
}

export default LoginMenu;
