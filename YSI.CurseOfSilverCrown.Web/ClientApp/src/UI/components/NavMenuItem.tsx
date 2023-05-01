import * as React from 'react';
import { Link } from 'react-router-dom';
import { NavItem, NavLink } from 'reactstrap';

interface INavItemProps {
    path: string,
    name: string
}

const NavMenuItem: React.FC<INavItemProps> = (props) => {
    return (
        <NavItem >
            <NavLink tag={Link} className="text-dark" to={props.path}>
                {props.name}
            </NavLink>
        </NavItem>
    )
}

export default NavMenuItem;
