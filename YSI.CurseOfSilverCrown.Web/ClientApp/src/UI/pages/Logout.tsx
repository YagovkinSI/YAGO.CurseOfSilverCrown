import * as React from 'react';
import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../store';
import { userActionCreators } from '../../store/User';

const Logout: React.FC = () => {    
    const state = useAppSelector(state => state.userReducer);
    const dispatch = useAppDispatch(); 

    useEffect(() => {
        if (!state.isLoading)
            userActionCreators.logout(dispatch)
    });

    return (
        <div>
            <h1>Выход...</h1>
        </div>
    )
};

export default Logout;
