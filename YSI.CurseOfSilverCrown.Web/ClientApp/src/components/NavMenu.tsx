import * as React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import LoginMenu from './LoginMenu';
import { AppDispatch } from '..';
import { useDispatch, useSelector } from 'react-redux';
import { ApplicationState } from '../store';
import NavMenuItem from './NavMenuItem';

const NavMenu: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const appState = useSelector(state => state as ApplicationState);

    const state = appState.user;

    const [isOpen, setIsOpen] = React.useState(false);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                <Container style={{ display: 'flex', justifyContent: 'space-between', flexWrap: 'wrap' }}>
                    <NavbarBrand tag={Link} to="/">Проклятие Серебряной Короны</NavbarBrand>
                    <NavbarToggler onClick={() => setIsOpen(!isOpen)} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row" isOpen={isOpen} navbar>
                        <ul className="navbar-nav flex-grow-1">
                            <NavMenuItem name='Главная' path='/' />
                            {
                                state != undefined && state.isSignedIn
                                    ?
                                    <NavMenuItem name='Владение' path='/Domain' />
                                    :
                                    <></>
                            }
                            <NavMenuItem name='Карта' path='/Map' />
                            <NavMenuItem name='История' path='/History' />
                            <NavMenuItem name='Список владений' path='/Organizations' />
                        </ul>
                        <LoginMenu />
                    </Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
