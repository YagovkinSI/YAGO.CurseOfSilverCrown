import * as React from 'react';
import { useAppSelector } from '../store';
import { NavItem, NavLink } from 'reactstrap';
import NavMenuItem from './NavMenuItem';


const LoginMenu: React.FC = () => {
    const state = useAppSelector(state => state.userReducer);

    if (state.isSignedIn) {
        return (
            <ul className="navbar-nav">
                <NavItem>
                    <NavLink className="text-dark">Здравствуйте, {state.userName}!</NavLink>
                </NavItem>
                <NavMenuItem name='Выйти' path='/Logout' />
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
