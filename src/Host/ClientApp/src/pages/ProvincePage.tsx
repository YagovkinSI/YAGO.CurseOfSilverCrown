import { Divider, Typography } from '@mui/material';
import YagoCard from '../shared/YagoCard';
import { type MapElement } from '../entities/MapData';
import { useGetMapDataQuery } from '../entities/MapData';
import { useParams } from 'react-router-dom';
import type YagoEnity from '../entities/YagoEnity';
import ErrorField from '../shared/ErrorField';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import LoadingCard from '../shared/LoadingCard';
import { YagoEntityTypeList } from '../entities/YagoEnity';
import { useGetProvinceWithUserQuery } from '../entities/provinces/ProvinceWithUser';

const ProvincePage: React.FC = () => {
    const { id } = useParams();
    const idAsNumber = id == 'up' || id == 'down'
        ? -1
        : id == null
            ? 0
            : parseInt(id, 10) || 0;

    const { data: mapElements, isLoading: isLoading1, error: error1 } = useGetMapDataQuery();
    const { data: provinceWithUser, isLoading: isLoading2, error: error2 } = idAsNumber == -1
        ? { data: undefined, isLoading: false, error: undefined }
        : useGetProvinceWithUserQuery(idAsNumber);
    let isLoading = isLoading1 || isLoading2;
    let error = error1 ?? error2;


    const unknownEarthEntity: YagoEnity = { id: -1, name: "Неигровая провинция", type: YagoEntityTypeList.Unknown };
    const unknownEarthMapElement: MapElement = {
        yagoEntity: unknownEarthEntity,
        info: [],
        colorStr: 'gray'
    }
    const province = idAsNumber == -1
        ? unknownEarthMapElement
        : mapElements?.[`${idAsNumber}`];

    const renderUser = () => {
        const userName = provinceWithUser?.user?.userName ?? '-';

        return (
            <Typography textAlign="justify" gutterBottom>Пользователь: {userName}</Typography>
        )
    }

    const renderProvinceContent = () => {
        return (
            <>
                <Divider />
                {renderUser()}
                <Divider />
                {province?.info.map(i =>
                    i == '<hr>'
                        ? <Divider />
                        : <Typography textAlign="justify" gutterBottom>{i}</Typography>
                )}
            </>
        )
    }

    const renderCard = () => {
        const path = province == undefined || province.yagoEntity.type == YagoEntityTypeList.Unknown
            ? undefined
            : `/Organizations/Details/${province.yagoEntity.id}`

        return (
            <YagoCard
                title={province?.yagoEntity.name ?? 'Неизвестная провинция'}
                path={path}
                isLinkToRazor={true}
            >
                {idAsNumber == -1 ? <></> : renderProvinceContent()}
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error == undefined && mapElements != undefined && province != undefined
                    ? renderCard()
                    : <DefaultErrorCard />}
        </>
    )
}

export default ProvincePage