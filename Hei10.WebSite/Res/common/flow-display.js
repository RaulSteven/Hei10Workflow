var myDiagram = {};
function initDiagram() {
    var $ = go.GraphObject.make;

    myDiagram =
        $(go.Diagram, "flowDiagram",
        {
            allowDrop: false,
            allowSelect: true,
            allowHorizontalScroll: true,
            allowVerticalScroll: true,
            allowMove: false,
            allowLink: false,
            allowRelink: false,
            "draggingTool.dragsLink": false
        });

    var nodeSelectionAdornmentTemplate =
        $(go.Adornment, "Auto",
            $(go.Shape, { fill: null, stroke: "deepskyblue", strokeWidth: 1.5, strokeDashArray: [4, 2] }),
            $(go.Placeholder)
        );

    myDiagram.nodeTemplate =
        $(go.Node, "Spot",
            { locationSpot: go.Spot.Center },
            new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
            { selectable: true, selectionAdornmentTemplate: nodeSelectionAdornmentTemplate },
            new go.Binding("angle").makeTwoWay(),
            $(go.Panel, "Auto",
                { name: "PANEL" },
                new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify),
                $(go.Shape, "RoundedRectangle", // default figure
                    {
                        portId: "", // the default port: if no spot on link data, use closest side
                        name: "PIPE",
                        fromLinkable: true,
                        toLinkable: true,
                        cursor: "pointer",
                        fill: "#7e7e7f", // default color
                        strokeWidth: 1,
                        stroke: "#DDDDDD"
                    },
                    new go.Binding("figure"),
                    new go.Binding("stroke"),
                    new go.Binding("strokeDashArray"),
                    new go.Binding("strokeWidth"),
                    new go.Binding("fill")),
                $(go.TextBlock,
                    {
                        font: "bold 11pt Helvetica, Arial, sans-serif",
                        margin: 8,
                        maxSize: new go.Size(160, NaN),
                        wrap: go.TextBlock.WrapFit,
                        stroke: "white"
                    },
                    new go.Binding("text").makeTwoWay()),
                    {
                        toolTip: $(go.Adornment, "Auto",
                                        $(go.Shape, { fill: "#FFFFCC" }),
                                        $(go.TextBlock, { margin: 4 },
                                                new go.Binding("text", "", nodeInfo))
                                )
                    }
            )
        );

    function nodeInfo(d) {
        if (!d.roleNames) return "无角色限制";
        return "角色：" + d.roleNames;
    }

    var linkSelectionAdornmentTemplate =
        $(go.Adornment, "Link",
            $(go.Shape,
            { isPanelMain: true, fill: null, stroke: "deepskyblue", strokeWidth: 0 })
        );

    myDiagram.linkTemplate =
        $(go.Link,
            { selectable: false, selectionAdornmentTemplate: linkSelectionAdornmentTemplate },
            { relinkableFrom: true, relinkableTo: true, reshapable: true },
            {
                routing: go.Link.AvoidsNodes,
                curve: go.Link.JumpOver,
                corner: 5,
                toShortLength: 4
            },
            new go.Binding("layerName", "color"),
            new go.Binding("zOrder"),
            $(go.Shape, { isPanelMain: true, stroke: "black", strokeWidth: 3 }, new go.Binding("stroke"),new go.Binding("zOrder")),
            $(go.Shape, { isPanelMain: true, stroke: "gray", strokeWidth: 2 }),
            $(go.Shape, { isPanelMain: true, stroke: "white", strokeWidth: 1, name: "PIPE", strokeDashArray: [10, 10] }),
            $(go.Shape,
            { toArrow: "standard", stroke: null }, new go.Binding("stroke"), new go.Binding("fill"), new go.Binding("zOrder")),
            $(go.Panel, "Auto",
                $(go.Shape, {
                    fill: null,
                    stroke: null
                }, new go.Binding("fill", "pFill"), new go.Binding("zOrder")),
                $(go.TextBlock,
                    {
                        textAlign: "center",
                        font: "10pt helvetica, arial, sans-serif",
                        stroke: "#555555",
                        margin: 4
                    },
                    new go.Binding("text", "text"), new go.Binding("zOrder"))
            )
        );


}

// 循环闪烁“已完成”步骤之间的连线
function loopLinks(links) {
    setTimeout(function () {
        updateFinishedLinks(links);// “已完成”连线
        loopLinks(links);
    }, 300);
}

// 循环闪烁“待处理”步骤
var loopWaitHandleNode = function (node) {
    setTimeout(function () {
        updateWaitHandleNode(node);
        loopWaitHandleNode(node);
    }, 200);
}

// 高亮“待处理”步骤
function updateWaitHandleNode(node) {
    if (!node) return;

    myDiagram.startTransaction("vacate");
    myDiagram.model.setDataProperty(node.data, "fill", (node.data.fill === "#ff9001") ? "#ffB001" : "#ff9001");
    myDiagram.commitTransaction("vacate");

    // 边框加上流水动画
    var shape = node.findObject("PIPE");
    var off = shape.strokeDashOffset - 2;
    shape.strokeDashOffset = (off <= 0) ? 20 : off;
}

// 返回所有【已完成】的步骤
var findFinishedSteps = function (stepKeys, isCompleted) {

    var arrStep = [];

    if (!stepKeys) return arrStep;

    var startStep = findStartStep();// 【开始】步骤
    arrStep.push(startStep);

    // 【已完成】的步骤
    var finishedCount = stepKeys.length - 1;// 不包含最后一个“待处理“步骤
    if (isCompleted) {
        finishedCount = stepKeys.length;// 包含所有步骤
    }
    for (var i = 0; i < finishedCount; i++) {
        var stepKey = stepKeys[i];
        var step = myDiagram.findNodeForKey(stepKey);
        if (!step) continue;

        arrStep.push(step);
    }
    return arrStep;
};

/**
 * 
 * 查找【开始】节点
 * @param {} steps 
 * @returns {} 
 */
var findStartStep = function () {
    var startStep = null;
    myDiagram.nodes.each(function(step) {

        if (step.data.hasOwnProperty('activityType') && step.data.activityType == 1) {
            startStep = step;
            return false;
        }
    });
    return startStep;
};

// 获取最后一个步骤（【待处理】或【结束】）
var findLastStep = function (stepKeys, steps, isCompleted) {
    var lastStep;
    if (!isCompleted) {
        // 获取“待处理”步骤
        var lastKey = stepKeys[stepKeys.length - 1];
        var step = myDiagram.findNodeForKey(lastKey);
        myDiagram.startTransaction("vacate");
        myDiagram.model.setDataProperty(step.data, "stroke", "red");
        myDiagram.model.setDataProperty(step.data, "strokeWidth", 2);
        myDiagram.model.setDataProperty(step.data, "strokeDashArray", [10, 10]);
        myDiagram.commitTransaction("vacate");

        //【开始】-> 【已完成】（N个）->【待处理】
        lastStep = step;
    } else {
        // 用最后一根连线获取【结束】步骤
        var lastFinishedStep = steps[steps.length - 1];

        var it = lastFinishedStep.findLinksOutOf();
        var lastLink = it.first();
        var endStep = lastLink.toNode;

        //【开始】-> 【已完成】（N个）->【结束】
        lastStep = endStep;
    }

    return lastStep;
};

// 查找步骤之间的连线
var findFinishedLinks = function (steps) {

    var arrLinks = [];

    if (!steps || steps.length < 1) return arrLinks;

    var currStep = steps[0];// 【开始】步骤

    for (var i = 0; i < steps.length; i++) {

        var step = steps[i];

        // 连线
        var link = currStep.findLinksBetween(step).first();
        if (!link) continue;
        arrLinks.push(link);

        currStep = step;
    }

    return arrLinks;
};

// 高亮所有“已完成”步骤的连线
var updateFinishedLinks = function (links) {

    if (!links) return;

    for (var i = 0; i < links.length; i++) {

        // 连线
        var link = links[i];
        myDiagram.startTransaction("vacate");
        myDiagram.model.setDataProperty(link.data, "stroke", (link.data.stroke === "#4fba4f" ? "red" : "#4fba4f"));
        myDiagram.model.setDataProperty(link.data, "fill", (link.data.fill === "#4fba4f" ? "red" : "#4fba4f"));
        myDiagram.model.setDataProperty(link.data, "zOrder", 999);
        myDiagram.commitTransaction("vacate");

        // 置于最上层，防止被遮挡
        myDiagram.startTransaction('modified zOrder');
        myDiagram.model.setDataProperty(link.data, "zOrder", 1);
        myDiagram.commitTransaction('modified zOrder');

        // 连线加上流水动画
        var shape = link.findObject("PIPE");
        var off = shape.strokeDashOffset - 2;
        shape.strokeDashOffset = (off <= 0) ? 20 : off;
    }
}

// 高亮“已完成”步骤
var updateFinishedNodes = function (steps) {

    if (!steps) return;

    for (var i = 0; i < steps.length; i++) {
        var step = steps[i];

        // 步骤
        myDiagram.startTransaction("vacate");
        myDiagram.model.setDataProperty(step.data, "fill", "#4fba4f");
        myDiagram.commitTransaction("vacate");
    }
}

// 加载流程图
function loadDiagram(flowData) {
    if (!flowData) return;

    myDiagram.model = go.Model.fromJson(flowData);
    var pos = myDiagram.model.modelData.position;
    if (pos) myDiagram.initialPosition = go.Point.parse(pos);

    // 更改所有连线中间的文本背景色
    myDiagram.links.each(function (link) {
        myDiagram.startTransaction("vacate");
        if (link.data.text) {
            myDiagram.model.setDataProperty(link.data, "pFill", window.go.GraphObject.make(go.Brush, "Radial", {
                0: "rgb(240, 240, 240)",
                0.3: "rgb(240, 240, 240)",
                1: "rgba(240, 240, 240, 0)"
            }));
        }
        myDiagram.commitTransaction("vacate");
    });
}