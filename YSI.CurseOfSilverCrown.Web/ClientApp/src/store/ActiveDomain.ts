import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { AppDispatch } from '.';
import { RequestType, RequestParams, requester } from '../requester';

export interface ActiveDomainState {
    activeDomain: IDomainPublic | undefined,
    isLoading: boolean,
    error: string
}

export interface IDomainPublic {
    id: number
    name: string
    info: string[]
}

export const defaultActiveDomainState: ActiveDomainState = {
    activeDomain: undefined,
    isLoading: false,
    error: ''
}

const request = requester.createThunk('activeDomain')

export const activeDomainSlice = createSlice({
    name: 'activeDomain',
    initialState: defaultActiveDomainState,
    reducers: {},
    extraReducers: {
        [request.fulfilled.type]: (state, action: PayloadAction<IDomainPublic>) => {
            state.activeDomain = action.payload,
                state.isLoading = false
        },
        [request.pending.type]: (state) => {
            state.activeDomain = undefined,
                state.isLoading = true
        },
        [request.rejected.type]: (state, action: PayloadAction<string>) => {
            state.activeDomain = undefined,
                state.error = action.payload,
                state.isLoading = false
        }
    }
});

const getDomainPublic = async (dispatch: AppDispatch, domainId: number) => {
    const requestParams: RequestParams = {
        path: 'apiDomain/getDomainPublic',
        type: RequestType.Get,
        data: { params: { domainId } }
    }
    dispatch(request(requestParams));
}

export const activeDomainActionCreators = { getDomainPublic };
