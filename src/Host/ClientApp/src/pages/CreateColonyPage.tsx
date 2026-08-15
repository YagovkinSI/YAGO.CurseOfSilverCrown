import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Page from '../widgets/Page';
import { useCreateColonyMutation, useGetMyColonyQuery } from '../entities/colonies/colony.api';
import LoadingCard from '../shared/ui/LoadingCard';
import { FlexContainer } from '../shared/ui/FlexContainer';

const CreateColonyPage: React.FC = () => {
    const navigate = useNavigate();

    const getMyColonyResult = useGetMyColonyQuery();
    const [createColony, createColonyResult] = useCreateColonyMutation();

    const isLoading = createColonyResult.isLoading;
    const error = createColonyResult.error;

    const fetchResult = async () => {
        await createColony().unwrap()
        navigate('/me/colony')};

    useEffect(() => {
            if (getMyColonyResult.isFetching)
                return;
            if (getMyColonyResult.data?.data != undefined)
                navigate('/me/colony');
            else
                fetchResult();
        }, [getMyColonyResult, createColony, navigate]);

    const renderContent = () => (
        <FlexContainer className="h-full p-2">
            <LoadingCard />
        </FlexContainer>
    );

    return (
        <Page backgroundImage={undefined} isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default CreateColonyPage;