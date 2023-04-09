import * as React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import LoginMenu from './LoginMenu';

const NavMenu: React.FC = () => {
    const [isOpen, setIsOpen] = React.useState(false);
    const [isSignedIn, setIsSignedIn] = React.useState(false);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                <Container style={{ display: 'flex', justifyContent: 'space-between', flexWrap: 'wrap' }}>
                    <NavbarBrand tag={Link} to="/">Проклятие Серебряной Короны</NavbarBrand>
                    <NavbarToggler onClick={() => setIsOpen(!isOpen)} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row" isOpen={isOpen} navbar>
                        <ul className="navbar-nav flex-grow-1">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/">Главная</NavLink>
                            </NavItem>
                            {
                                isSignedIn
                                    ?
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/Domain">Владение</NavLink>
                                    </NavItem>
                                    :
                                    <></>
                            }
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/Map">Карта</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/History">История</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/Organizations">Список владений</NavLink>
                            </NavItem>
                        </ul>
                        <LoginMenu />
                    </Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
