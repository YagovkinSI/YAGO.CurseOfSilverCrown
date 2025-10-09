import type { SvgIconTypeMap } from "@mui/material";
import type { OverridableComponent } from "@mui/material/OverridableComponent";

export interface StateItem {
    color: string,
    icon: OverridableComponent<SvgIconTypeMap<{}, "svg">> & { muiName: string; },
    label: string,
    value: string | number
}