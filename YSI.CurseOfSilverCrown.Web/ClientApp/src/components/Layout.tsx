import * as React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import ErrorField from './ErrorField';

export interface LayoutProps {
    children?: React.ReactNode;
}
const Layout: React.FC<LayoutProps> = ({ children }) => {
    return (
        <React.Fragment>
            <NavMenu />
            <ErrorField />   
            <Container>
                <main>
                    {children}
                </main>
            </Container>
            <footer className="border-top footer text-muted">
                <div className="container" style={{ textAlign: 'center' }}>
                    &copy; Яговкин С.И., 2021—2023
                </div>
            </footer>
        </React.Fragment>
    );
}

export default Layout;