import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { AppDispatch } from '.';
import { RequestType, RequestParams, requester } from '../requester';

export interface DomainListState {
    domainPublicList?: DomainPublic[],
    isLoading: boolean,
    error: string
}

export interface DomainPublic {
    id: number
    name: string
    warriors: number
    gold: number
    investments: number
    fortifications: number
    suzerain: DomainLink 
    vassalCount: number
    user: UserLink
}

export interface DomainLink {
    id: number
    name: string
}

export interface UserLink {
    id: string
    name: string
}

export const defaultDomainListState: DomainListState = {
    domainPublicList: undefined,
    isLoading: false,
    error: ''
}

const request = requester.createThunk('domainList')

export const domainListSlice = createSlice({
    name: 'domainList',
    initialState: defaultDomainListState,
    reducers: {},
    extraReducers: {
        [request.fulfilled.type]: (state, action: PayloadAction<DomainPublic[]>) => {
            state.domainPublicList = action.payload,
                state.isLoading = false
        },
        [request.pending.type]: (state) => {
            state.domainPublicList = undefined,
                state.isLoading = true
        },
        [request.rejected.type]: (state, action: PayloadAction<string>) => {
            state.domainPublicList = undefined,
                state.error = action.payload,
                state.isLoading = false
        }
    }
});

const getDomainList = async (dispatch: AppDispatch, column?: number) => {
    const requestParams: RequestParams = {
        path: 'apiDomain/getDomainList',
        type: RequestType.Get,
        data: { params: { column } }
    }
    dispatch(request(requestParams));
}

export const domainListActionCreators = { getDomainList };
