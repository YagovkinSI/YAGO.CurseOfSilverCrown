import * as React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { AppDispatch } from '..';
import { ApplicationState } from '../store';
import { NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';


const LoginMenu: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const appState = useSelector(state => state as ApplicationState);

    const state = appState.user;

    if (state != undefined && state.isSignedIn) {
        return (
            <ul className="navbar-nav">
                <NavItem>
                    <NavLink className="text-dark">Здравствуйте, {state.userName}!</NavLink>
                </NavItem><NavItem>
                    <NavLink tag={Link} className="text-dark" to="/Leave">Выйти</NavLink>
                </NavItem>
            </ul>
        );
    }
    else {
        return (
            <ul className="navbar-nav">
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/Register">Регистрация</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/Login">Вход</NavLink>
                </NavItem>
            </ul>
        );
    }
}

export default LoginMenu;
